using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.Services;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationBackEnd.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private Context _context;
        private readonly ILogger<StudentController> _logger;
        private readonly ILogger<AuthController> _logger1;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(Context context, IConfiguration configuration, ILogger<StudentController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = new UnitOfWork(context);
        }

        // GET: api/Student
        [HttpGet("DetailedStudentsInformation")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Student>>> GetDetailedStudents()
        {
            return Ok(await _unitOfWork.Students.GetAll());
        }

        // GET: api/Student
        [HttpGet("GeneralStudentsInformation")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<IEnumerable<ReadStudentRestrictedDto>>> GetRestrictedStudents()
        {
            List<ReadStudentRestrictedDto> listReadStudentRestrictedDto = new List<ReadStudentRestrictedDto>();
            
            foreach(Student student in await _unitOfWork.Students.GetAll())
            {
                ReadStudentRestrictedDto readStudentRestrictedDto = new ReadStudentRestrictedDto()
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    StudentSurname = student.StudentSurname,
                    StudentEmail = student.StudentEmail,
                    StudentPhone = student.StudentPhone,
                    StudentTotalCourseCredits = student.StudentTotalCourseCredits,
                    StudentGeneralAverageMark = student.StudentGeneralAverageMark
                };
                listReadStudentRestrictedDto.Add(readStudentRestrictedDto);
            }

            return Ok(listReadStudentRestrictedDto);
        }

        //THIS METHOD WILL BE CONTROLLED TO CHECK WHETHER STUDENT'S AUTHORIZATION IS WORKING PROPERLY.
        [HttpGet("{studentId:int}/DetailedStudentInformation")]
        [Authorize(Roles = "Admin, Manager, Student")]
        public async Task<ActionResult<ReadStudentDto>> GetDetailedStudent(int studentId)
        {
            var student = await _unitOfWork.Students.Get(studentId);
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;

            if (student == null)
            {
                return NotFound();
            }
            
            if(student.StudentUserName != userName && userRole == "Student")
            {
                return BadRequest("You are not allowed to query this student's information!");
            }

            ReadStudentDto readStudentDto = new ReadStudentDto()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                StudentSurname = student.StudentSurname,
                StudentUserName = student.StudentUserName,
                StudentEmail = student.StudentEmail,
                StudentPhone = student.StudentPhone,
                StudentTotalCourseCredits = student.StudentTotalCourseCredits,
                StudentGeneralAverageMark = student.StudentGeneralAverageMark,
                StudentDebts = student.StudentDebts,
            };

            return readStudentDto;
        }

        //THIS METHOD WILL BE CONTROLLED TO CHECK WHETHER STUDENT'S AUTHORIZATION IS WORKING PROPERLY.
        [HttpGet("{studentId:int}/GeneralStudentInformation")]
        [Authorize(Roles = "Admin, Manager, Teacher, Student")]
        public async Task<ActionResult<ReadStudentRestrictedDto>> GetRestrictedStudent(int studentId)
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

            ReadStudentRestrictedDto readStudentRestrictedDto = new ReadStudentRestrictedDto()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                StudentSurname = student.StudentSurname,
                StudentEmail = student.StudentEmail,
                StudentPhone = student.StudentPhone,
                StudentTotalCourseCredits = student.StudentTotalCourseCredits,
                StudentGeneralAverageMark = student.StudentGeneralAverageMark
            };
            return readStudentRestrictedDto;
        }

        [HttpGet("DebtsOfStudents")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<List<ReadStudentDto>>> GetDebtsOfStudent()
        {
            List<ReadStudentDto> readStudentDto= new List<ReadStudentDto>();

            foreach (Student student in await _unitOfWork.Students.GetAll())
            {
                readStudentDto.Add(new ReadStudentDto()
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    StudentSurname = student.StudentSurname,
                    StudentEmail = student.StudentEmail,
                    StudentPhone = student.StudentPhone,
                    StudentTotalCourseCredits = student.StudentTotalCourseCredits,
                    StudentGeneralAverageMark = student.StudentGeneralAverageMark,
                    StudentDebts = student.StudentDebts,
                });
            }

            if (readStudentDto == null)
            {
                return NotFound("There isn't any student record in Students!");
            }

            return readStudentDto;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{studentId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PutStudent(int studentId, UpdateStudentDto updateStudentDto)
        {
            var student = await _unitOfWork.Students.Get(studentId);

            if (student == null)
                return BadRequest("A student couldn't be found with given studentId!");

            student.StudentName = updateStudentDto.StudentName;
            student.StudentSurname = updateStudentDto.StudentSurname;
            student.StudentEmail = updateStudentDto.StudentEmail;
            student.StudentPhone = updateStudentDto.StudentPhone;
            student.StudentDebts = updateStudentDto.StudentDebts;
            _unitOfWork.Students.Update(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(studentId))
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

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Student>> PostStudent(CreateStudentDto request)
        {
            if (IsDigitsOnly(request.StudentPhone) == false)
            {
                return BadRequest("Phone Number can have just 0-9 numbers and can just have 10 digits length!");
            }

            Student student = new Student()
            {
                StudentName = request.StudentName,
                StudentSurname = request.StudentSurname,
                StudentEmail = request.StudentEmail,
                StudentPhone = request.StudentPhone,
                StudentTotalCourseCredits = 0,
                StudentGeneralAverageMark = 0,
                StudentDebts = 0,
                StudentCreatedDate = DateTime.Now
            };

            string studentUserName = Convert.ToString(student.StudentName[0]);
            studentUserName += student.StudentSurname + "22";
            studentUserName = studentUserName.ToLower();
            studentUserName = TurkishCharacterToEnglish(studentUserName);
            int i = 0;

            while (true)
            {
                i++;
                SchoolUser existingUserControl = await _unitOfWork.SchoolUsers.Get(studentUserName);
                if (existingUserControl != null)
                {
                    if (i > 1)
                        studentUserName.Remove(studentUserName.Length - 1, 1);
                    studentUserName += Convert.ToString(i);
                }
                else
                    break;
            }

            student.StudentUserName = studentUserName;
            CreateSchoolUserDto createUserDto = new CreateSchoolUserDto()
            {
                UserName = studentUserName,
                Password = studentUserName,
                UserRole = "Student",
            };

            await _unitOfWork.Students.Add(student);
            _unitOfWork.Complete();
            var result = await new AuthController(_context, _configuration, _logger1).Register(createUserDto);

            return CreatedAtAction("GetDetailedStudent", new { studentId = student.StudentId}, "The new student has been successfully recorded to the database!");
        }

        // DELETE: api/Student/5
        [HttpDelete("{studentId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var Student = await _unitOfWork.Students.Get(studentId);

            if (Student == null)
            {
                return NotFound();
            }

            await _unitOfWork.SchoolUsers.Remove(Student.StudentUserName);
            await _unitOfWork.Students.Remove(studentId);
            _unitOfWork.Complete();

            return NoContent();
        }

        private bool StudentExists(int studentId)
        {
            return (_context.Students?.Any(e => e.StudentId == studentId)).GetValueOrDefault();
        }
        private string TurkishCharacterToEnglish(string text)
        {
            char[] turkishChars = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö', 'ü', 'Ü' };
            char[] englishChars = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U' };

            // Match chars
            for (int i = 0; i < turkishChars.Length; i++)
                text = text.Replace(turkishChars[i], englishChars[i]);

            return text;
        }
        private bool IsDigitsOnly(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (c < '0' || c > '9' || phoneNumber.Length != 10)
                    return false;
            }

            return true;
        }
    }
}
