using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.DTO;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<StudentCourseController> _logger;
        private IUnitOfWork _unitOfWork;

        public StudentCourseController(Context context, ILogger<StudentCourseController> logger)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: api/StudentCourse
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<ReadStudentCourseDto>>> GetStudentCourses()
        {
          if (_context.StudentsCourses == null)
          {
              return NotFound();
          }
          List<ReadStudentCourseDto> StudentCoursesDtoList = new List<ReadStudentCourseDto>();
          foreach (StudentCourse studentCourse in await _unitOfWork.StudentsCourses.GetAll())
            {
                Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);

                ReadStudentCourseDto tempStudentCourseDto = new ReadStudentCourseDto()
                {
                    StudentCourseId = studentCourse.StudentCourseId,
                    StudentMidTermMark = studentCourse.StudentMidTermMark,
                    StudentFinalMark = studentCourse.StudentFinalMark,
                    StudentAverageMark = studentCourse.StudentAverageMark,
                    StudentId = studentCourse.StudentId,
                    CourseId = studentCourse.CourseId,
                    TeacherId = course.TeacherId
                };
                StudentCoursesDtoList.Add(tempStudentCourseDto);
            }
            return StudentCoursesDtoList;
        }

        // GET: api/StudentCourse/5
        [HttpGet("{studentCourseId:int}")]
        [Authorize(Roles = "Admin, Manager, Teacher, Student")]
        public async Task<ActionResult<ReadStudentCourseDto>> GetStudentCourse(int studentCourseId)
        {
            if (_context.StudentsCourses == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentsCourses.FindAsync(studentCourseId);
            var student = await _context.Students.FindAsync(studentCourse.StudentId);

            if (studentCourse == null)
            {
                return NotFound();
            }

            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;
            
            if (student.StudentUserName != userName && userRole == "Student")
            {
                return BadRequest("You are not allowed for this query due to you aren't the student of given StudentCourse relationship!");
            }

            Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);

            ReadStudentCourseDto readStudentCourseDto = new ReadStudentCourseDto()
            {
                StudentCourseId = studentCourse.StudentCourseId,
                StudentMidTermMark = studentCourse.StudentMidTermMark,
                StudentFinalMark = studentCourse.StudentFinalMark,
                StudentAverageMark = studentCourse.StudentAverageMark,
                StudentId = studentCourse.StudentId,
                CourseId = studentCourse.CourseId,
                TeacherId = course.TeacherId
            };
            return readStudentCourseDto;
        }

        [HttpGet("{studentId:int}/StudentCoursesOfStudent")]
        [Authorize(Roles = "Admin, Manager, Teacher, Student")]
        public async Task<ActionResult<List<ReadStudentCourseDto>>> GetStudentCoursesOfStudent(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;
            
            if (userRole == "Student" && student.StudentUserName != userName)
            {
                return BadRequest("You are not allowed for this query due to you aren't the student of given StudentCourse relationship!");
            }

            List<ReadStudentCourseDto> readStudentCourses = new List<ReadStudentCourseDto>();

            foreach(StudentCourse studentCourse in await _unitOfWork.StudentsCourses.GetAll())
            {
                Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);
                
                if (studentCourse.StudentId == studentId)
                {
                    readStudentCourses.Add(new ReadStudentCourseDto()
                    {
                        StudentCourseId = studentCourse.StudentCourseId,
                        StudentMidTermMark = studentCourse.StudentMidTermMark,
                        StudentFinalMark = studentCourse.StudentFinalMark,
                        StudentAverageMark = studentCourse.StudentAverageMark,
                        StudentId = studentCourse.StudentId,
                        CourseId = studentCourse.CourseId,
                        TeacherId = course.TeacherId
                    });                   
                }
            }

            if (readStudentCourses == null)
            {
                return NotFound();
            }
            
            return readStudentCourses;
        }

        [HttpGet("{studentId:int}/CoursesOfStudent")]
        [Authorize(Roles = "Admin, Manager, Teacher, Student")]
        public async Task<ActionResult<List<Course>>> GetCoursesOfStudent(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (userRole == "Student" && student.StudentUserName != userName)
            {
                return BadRequest("You are not allowed for this query due to you aren't the student of given StudentCourse relationship!");
            }

            List<Course> CoursesOfStudent= new List<Course>();

            foreach (StudentCourse studentCourse in await _unitOfWork.StudentsCourses.GetAll())
            {
                if (studentCourse.StudentId == studentId)
                    CoursesOfStudent.Add(await _unitOfWork.Courses.Get(studentCourse.CourseId));
            }

            if (CoursesOfStudent.Count == 0)
            {
                return NotFound("Given student isn't registered to any course!");
            }

            return CoursesOfStudent;
        }

        [HttpGet("{courseId:int}/StudentCoursesOfCourse")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<List<ReadStudentCourseDto>>> GetStudentsCoursesOfCourse(int courseId)
        {
            Course course = await _unitOfWork.Courses.Get(courseId);

            if (course == null)
                return BadRequest("A course couldn't be found with given courseId!");

            Teacher teacher = await _unitOfWork.Teachers.Get(course.TeacherId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (userRole == "Teacher" && teacher.TeacherUserName != userName)
            {
                return BadRequest("You are not allowed for this query due to you aren't the teacher of given StudentCourse's course!");
            }

            List<ReadStudentCourseDto> readStudentCourses = new List<ReadStudentCourseDto>();

            foreach (StudentCourse studentCourse in await _unitOfWork.StudentsCourses.GetAll())
            {
                Course tempCourse = await _unitOfWork.Courses.Get(studentCourse.CourseId);

                if (studentCourse.CourseId == courseId)
                {
                    readStudentCourses.Add(new ReadStudentCourseDto()
                    {
                        StudentCourseId = studentCourse.StudentCourseId,
                        StudentMidTermMark = studentCourse.StudentMidTermMark,
                        StudentFinalMark = studentCourse.StudentFinalMark,
                        StudentAverageMark = studentCourse.StudentAverageMark,
                        StudentId = studentCourse.StudentId,
                        CourseId = studentCourse.CourseId,
                        TeacherId = tempCourse.TeacherId
                    });
                }
            }

            if (readStudentCourses == null)
            {
                return NotFound();
            }
            return readStudentCourses;
        }

        [HttpGet("{courseId:int}/StudentsOfCourse")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<List<ReadStudentRestrictedDto>>> GetStudentsOfCourse(int courseId)
        {
            try
            {
                Course course = await _unitOfWork.Courses.Get(courseId);
                Teacher teacher = await _unitOfWork.Teachers.Get(course.TeacherId);

                string userName = User.FindFirst(ClaimTypes.Name).Value;
                string userRole = User.FindFirst(ClaimTypes.Role).Value;

                if (userRole == "Teacher" && teacher.TeacherUserName != userName)
                {
                    return BadRequest("You are not allowed for this query due to you aren't the teacher of given StudentCourse's course!");
                }
            }
            catch(NullReferenceException nullReferenceExceptionError)
            {
                return BadRequest("A course couldn't be found with given courseId!");
            }

            List<ReadStudentRestrictedDto> listReadStudentRestrictedDto= new List<ReadStudentRestrictedDto>();

            foreach (StudentCourse studentCourse in await _unitOfWork.StudentsCourses.GetAll())
            {
                if (studentCourse.CourseId == courseId)
                {
                    Student student = await _unitOfWork.Students.Get(studentCourse.StudentId);
                    ReadStudentRestrictedDto readStudentRestrictedDto = new ReadStudentRestrictedDto()
                    {
                        StudentId = student.StudentId,
                        StudentName = student.StudentName,
                        StudentSurname = student.StudentSurname,
                        StudentEmail = student.StudentEmail,
                        StudentPhone = student.StudentPhone,
                        StudentGeneralAverageMark = student.StudentGeneralAverageMark
                    };

                    listReadStudentRestrictedDto.Add(readStudentRestrictedDto);
                }
            }

            if (listReadStudentRestrictedDto.Count == 0)
            {
                return NotFound();
            }

            return listReadStudentRestrictedDto;
        }

        // PUT: api/StudentCourse/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{studentCourseId:int}")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<IActionResult> PutStudentCourse(int studentCourseId, UpdateStudentCourseDto updateStudentCourseDto)
        {
            StudentCourse studentCourse = null;
            Course course = null;

            try
            {
                try
                {
                    studentCourse = await _unitOfWork.StudentsCourses.Get(studentCourseId);

                    if(studentCourse == null)
                        return BadRequest("Requested StudentCourse couldn't be found with given id!");
                }
                catch(NullReferenceException nullReferenceExceptionError)
                {
                    return BadRequest("Requested StudentCourse couldn't be found with given id!");
                }

                course = await _unitOfWork.Courses.Get(studentCourse.CourseId);
                Teacher teacher = await _unitOfWork.Teachers.Get(course.TeacherId);

                string userName = User.FindFirst(ClaimTypes.Name).Value;
                string userRole = User.FindFirst(ClaimTypes.Role).Value;

                if (userRole == "Teacher" && teacher.TeacherUserName != userName)
                {
                    return BadRequest("You are not allowed for this query due to you aren't the teacher of given StudentCourse's course!");
                }
            }
            catch (NullReferenceException nullReferenceExceptionError)
            {
                return BadRequest("A course couldn't be found with given courseId!");
            }

            if (updateStudentCourseDto.StudentMidTermMark < 0 || updateStudentCourseDto.StudentMidTermMark > 100 || updateStudentCourseDto.StudentFinalMark < 0 || updateStudentCourseDto.StudentFinalMark > 100)
                return BadRequest("A mark's range can just be between 0 and 100!");

            studentCourse.StudentId = updateStudentCourseDto.StudentId;
            studentCourse.StudentMidTermMark = updateStudentCourseDto.StudentMidTermMark;
            studentCourse.StudentFinalMark = updateStudentCourseDto.StudentFinalMark;
            studentCourse.StudentAverageMark = (studentCourse.StudentMidTermMark + studentCourse.StudentFinalMark) / 2;
            studentCourse.CourseId = updateStudentCourseDto.CourseId;
            _context.Entry(studentCourse).State = EntityState.Modified;
            _unitOfWork.Complete();

            return NoContent();
        }

        // POST: api/StudentCourse
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(CreateStudentCourseDto createStudentCourseDto)
        {
            Student student = await _unitOfWork.Students.Get(createStudentCourseDto.StudentId);
            Course course = await _unitOfWork.Courses.Get(createStudentCourseDto.CourseId);
            
            if (student == null || course == null)
                return BadRequest("Student or Course couldn't be found. One of both CourseId is invalid! ");

            StudentCourse studentCourse = new StudentCourse()
            {
                StudentId = student.StudentId,
                StudentMidTermMark = -1,
                StudentFinalMark = -1,
                StudentAverageMark = -1,
                Course = course,
                CourseId = course.CourseId,
            };

            List<int> CourseDates = CreateCourseDto.DayAdjuster(course.CourseDays);

            for (int i = 0; i < course.CourseNumbers; i++) //It creates as many Attendances as CourseNumbers
            {
                int extraDays = CourseDates.ElementAt(i % CourseDates.Count);
                extraDays += (i / CourseDates.Count) * 7;
                var tempAttendance = new Attendance()
                {
                    AttendanceDate = course.CourseStartDate.AddDays(extraDays),
                    AttendanceState = false,
                    AttendancePaymentState = false,
                };
                tempAttendance.StudentCourse = studentCourse;
                await _unitOfWork.Attendances.Add(tempAttendance);
            }

            await _unitOfWork.StudentsCourses.Add(studentCourse);
            student.StudentTotalCourseCredits += course.CourseCredits;
            student.StudentDebts += course.CourseTotalPrice;
            _context.Entry(student).State = EntityState.Modified;
            _unitOfWork.Complete();

            return CreatedAtAction("GetStudentCourse", new { studentCourseId = studentCourse.StudentCourseId}, "The new StudentCourse relationship has been successfully recorded to the database!");
        }

        // DELETE: api/StudentCourse/5
        [HttpDelete("{studentCourseId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteStudentCourse(int studentCourseId)
        {
            if (_context.StudentsCourses == null)
            {
                return NotFound();
            }
            
            StudentCourse studentCourse = await _context.StudentsCourses.FindAsync(studentCourseId);
            Student student = await _unitOfWork.Students.Get(studentCourse.StudentId);
            Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);
            student.StudentTotalCourseCredits -= course.CourseCredits;
            
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.Entry(student).State = EntityState.Modified;
            _context.StudentsCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCourseExists(int id)
        {
            return (_context.StudentsCourses?.Any(e => e.StudentCourseId == id)).GetValueOrDefault();
        }
    }
}
