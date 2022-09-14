using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTrackingApplicationBackEnd.Services;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationBackEnd.DTO;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private Context _context;
        private readonly ILogger<AttendanceController> _logger;
        private IUnitOfWork _unitOfWork;

        public AttendanceController(Context context, ILogger<AttendanceController> logger)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = new UnitOfWork(context);
        }

        // GET: api/Attendance
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<ReadAttendanceDto>>> GetAttendances()
        {
            List<ReadAttendanceDto> Attendances = new List<ReadAttendanceDto>();
            foreach(Attendance attendance in await _unitOfWork.Attendances.GetAll()){
                StudentCourse tempStudentCourse = await _unitOfWork.StudentsCourses.Get(attendance.StudentCourseId);
                Course course = await _unitOfWork.Courses.Get(tempStudentCourse.CourseId);
                
                Attendances.Add(new ReadAttendanceDto()
                {
                    AttendanceId = attendance.AttendanceId,
                    AttendanceDate = attendance.AttendanceDate,
                    AttendancePrice = attendance.AttendancePrice,
                    AttendanceState = attendance.AttendanceState,
                    AttendancePaymentState = attendance.AttendancePaymentState,
                    StudentCourseId = attendance.StudentCourseId,
                    StudentId = tempStudentCourse.StudentId,
                    CourseId = tempStudentCourse.CourseId,
                    TeacherId = course.TeacherId 
                });
            }
            return Ok(Attendances);
        }

        // GET: api/Attendance/5
        [HttpGet("{attendanceId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendance(int attendanceId)
        {
            Attendance attendance = await _unitOfWork.Attendances.Get(attendanceId);
            StudentCourse tempStudentCourse = await _unitOfWork.StudentsCourses.Get(attendance.StudentCourseId);

            if (attendance == null)
            {
                return NotFound();
            }

            Course course = await _unitOfWork.Courses.Get(tempStudentCourse.CourseId);

            ReadAttendanceDto AttendanceDto = new ReadAttendanceDto()
            {
                AttendanceId = attendance.AttendanceId,
                AttendanceDate = attendance.AttendanceDate,
                AttendancePrice = attendance.AttendancePrice,
                AttendanceState = attendance.AttendanceState,
                AttendancePaymentState = attendance.AttendancePaymentState,
                StudentCourseId = attendance.StudentCourseId,
                StudentId = tempStudentCourse.StudentId,
                CourseId = tempStudentCourse.CourseId,
                TeacherId = course.TeacherId
            };
            
            return AttendanceDto;
        }

        [HttpGet("{studentId:int}/AttendancesOfStudent")]
        [Authorize(Roles = "Admin, Manager, Student")]
        public async Task<ActionResult<List<ReadAttendanceDto>>> GetAttendancesOfStudent(int studentId)
        {
            var student = await _unitOfWork.Students.Get(studentId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (student == null)
            {
                return NotFound();
            }

            if (student.StudentUserName != userName && userRole == "Student")
            {
                return BadRequest("You are not allowed to query this student's information!");
            }

            List<ReadAttendanceDto> readAttendanceDto= new List<ReadAttendanceDto>();

            foreach (Attendance attendance in await _unitOfWork.Attendances.GetAll())
            {
                StudentCourse studentCourse = await _unitOfWork.StudentsCourses.Get(attendance.StudentCourseId);
                Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);

                if (studentCourse.StudentId == studentId)
                {
                    readAttendanceDto.Add(new ReadAttendanceDto()
                    {
                        AttendanceId = attendance.AttendanceId,
                        AttendanceDate = attendance.AttendanceDate,
                        AttendancePrice = attendance.AttendancePrice,
                        AttendanceState = attendance.AttendanceState,
                        AttendancePaymentState = attendance.AttendancePaymentState,
                        StudentCourseId = studentCourse.StudentCourseId,
                        StudentId = studentCourse.StudentId,
                        CourseId = studentCourse.CourseId,
                        TeacherId = course.TeacherId
                    });
                }
            }

            if (readAttendanceDto == null)
            {
                return NotFound();
            }

            return readAttendanceDto;
        }

        [HttpGet("{courseId:int}/AttendancesOfCourse")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<List<ReadAttendanceDto>>> GetAttendancesOfCourse(int courseId)
        {
            Course course = await _unitOfWork.Courses.Get(courseId);

            if (course == null)
                return BadRequest("A course couldn't be found with given courseId!");

            Teacher teacher = await _unitOfWork.Teachers.Get(course.TeacherId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (userRole == "Teacher" && teacher.TeacherUserName != userName)
            {
                return BadRequest("You are not allowed for this query due to you aren't the teacher of given Attendance's course!");
            }

            List<ReadAttendanceDto> readAttendanceDto = new List<ReadAttendanceDto>();

            foreach (Attendance attendance in await _unitOfWork.Attendances.GetAll())
            {
                StudentCourse studentCourse = await _unitOfWork.StudentsCourses.Get(attendance.StudentCourseId);
                Course tempCourse = await _unitOfWork.Courses.Get(studentCourse.CourseId);

                if (studentCourse.CourseId == courseId)
                {
                    readAttendanceDto.Add(new ReadAttendanceDto()
                    {
                        AttendanceId = attendance.AttendanceId,
                        AttendanceDate = attendance.AttendanceDate,
                        AttendancePrice = attendance.AttendancePrice,
                        AttendanceState = attendance.AttendanceState,
                        AttendancePaymentState = attendance.AttendancePaymentState,
                        StudentCourseId = studentCourse.StudentCourseId,
                        StudentId = studentCourse.StudentId,
                        CourseId = studentCourse.CourseId,
                        TeacherId = tempCourse.TeacherId
                    });
                }
            }

            if (readAttendanceDto == null)
            {
                return NotFound();
            }

            return readAttendanceDto;
        }

        [HttpPut("{attendanceId:int}/DetailedUpdate")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PutAttendanceForAdmin(int attendanceId, UpdateAttendanceGeneralUpdateDto updateAttendanceDto)
        {
            Attendance attendance = await _unitOfWork.Attendances.Get(attendanceId);
            
            if (attendance == null)
            {
                return BadRequest("Requested Attendance couldn't be found with given Id!");
            }

            attendance.AttendanceDate = updateAttendanceDto.AttendanceDate;
            attendance.AttendanceState = updateAttendanceDto.AttendanceState;
            attendance.AttendancePaymentState = updateAttendanceDto.AttendancePaymentState;

            _unitOfWork.Attendances.Update(attendance);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(attendanceId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{attendanceId:int}/ForTeacher")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<IActionResult> PutAttendance(int attendanceId, UpdateAttendanceForTeacher updateAttendanceDto)
        {
            Attendance attendance = await _unitOfWork.Attendances.Get(attendanceId);
            
            if (attendance == null)
            {
                return BadRequest("Requested Attendance couldn't be found with given Id!");
            }

            StudentCourse studentCourse = await _unitOfWork.StudentsCourses.Get(attendance.StudentCourseId);
            
            if (studentCourse == null)
                return BadRequest("A StudentCourse relationship couldn't be found with given attendanceId!");

            Course course = await _unitOfWork.Courses.Get(studentCourse.CourseId);

            if (course == null)
                return BadRequest("A course couldn't be found with given courseId!");

            Teacher teacher = await _unitOfWork.Teachers.Get(course.TeacherId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (userRole == "Teacher" && teacher.TeacherUserName != userName)
            {
                return BadRequest("You are not allowed for this query due to you aren't the teacher of given Attendance's course!");
            }

            attendance.AttendanceState = updateAttendanceDto.AttendanceState;
            _unitOfWork.Attendances.Update(attendance);
            _unitOfWork.Complete();

            return NoContent();
        }

        private bool AttendanceExists(int attendanceId)
        {
            return (_context.Attendances?.Any(e => e.AttendanceId == attendanceId)).GetValueOrDefault();
        }
    }
}
