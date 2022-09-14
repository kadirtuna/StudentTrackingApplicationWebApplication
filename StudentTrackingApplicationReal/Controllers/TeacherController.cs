using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationBackEnd.DTO;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.Services;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager")]
    public class TeacherController : ControllerBase
    {
        private readonly Context _context;
        private IConfiguration _configuration;
        private ILogger<TeacherController> _logger;
        private ILogger<AuthController> _logger1;
        private IUnitOfWork _unitOfWork;

        public TeacherController(Context context, IConfiguration configuration, ILogger<TeacherController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
          if (_context.Teachers == null)
          {
              return NotFound();
          }
            return await _context.Teachers.ToListAsync();
        }

        // GET: api/Teacher/5
        [HttpGet("{teacherId}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int teacherId)
        {
          if (_context.Teachers == null)
          {
              return NotFound();
          }
            var teacher = await _unitOfWork.Teachers.Get(teacherId);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/Teacher/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{teacherId:int}")]
        public async Task<IActionResult> PutTeacher(int teacherId, CreateTeacherDto createTeacherDto)
        {
            if (IsDigitsOnly(createTeacherDto.TeacherPhone) == false)
            {
                return BadRequest("Phone Number can just have between the range of 0 to 9 numbers and can just have 10 digits length!");
            }

            Teacher teacher = await _unitOfWork.Teachers.Get(teacherId);

            if (teacher == null)
            {
                return BadRequest("This Teacher couldn't be found with given TeacherId!");
            }

            teacher.TeacherName = createTeacherDto.TeacherName;
            teacher.TeacherSurname = createTeacherDto.TeacherSurname;
            teacher.TeacherEmail = createTeacherDto.TeacherEmail;
            teacher.TeacherPhone = createTeacherDto.TeacherPhone;
            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacherId))
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

        // POST: api/Teacher
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(CreateTeacherDto createTeacherDto)
        {
            if (IsDigitsOnly(createTeacherDto.TeacherPhone) == false)
            {
                return BadRequest("Phone Number can have just 0-9 numbers and can just have 10 digits length!");
            }

            Teacher teacher = new Teacher()
            {
                TeacherName = createTeacherDto.TeacherName,
                TeacherSurname = createTeacherDto.TeacherSurname,
                TeacherEmail = createTeacherDto.TeacherEmail,
                TeacherPhone = createTeacherDto.TeacherPhone,
                TeacherCreatedDate = DateTime.Now
            };

            string teacherUserName = Convert.ToString(teacher.TeacherName[0]);
            teacherUserName += teacher.TeacherSurname;
            teacherUserName = teacherUserName.ToLower();
            teacherUserName = TurkishCharacterToEnglish(teacherUserName);
            int i = 0;

            while (true)
            {
                i++;
                SchoolUser existingUserControl = await _unitOfWork.SchoolUsers.Get(teacherUserName);
                if (existingUserControl != null)
                {
                    if (i > 1)
                        teacherUserName.Remove(teacherUserName.Length - 1, 1);
                    teacherUserName += Convert.ToString(i);
                }
                else
                    break;
            }

            ReadTeacherDto readTeacherDto = new ReadTeacherDto()
            {
                TeacherName = teacher.TeacherName,
                TeacherSurname = teacher.TeacherSurname,
                TeacherUserName = teacherUserName,
                TeacherEmail = teacher.TeacherEmail,
                TeacherPhone = teacher.TeacherPhone,
            };

            teacher.TeacherUserName = teacherUserName;
            CreateSchoolUserDto createUserDto = new CreateSchoolUserDto()
            {
                UserName = teacherUserName,
                Password = teacherUserName,
                UserRole = "Teacher",
            };

            await _unitOfWork.Teachers.Add(teacher);
            var result = await new AuthController(_context, _configuration, _logger1).Register(createUserDto);
            _unitOfWork.Complete();

            return CreatedAtAction("GetTeacher", new { teacherId = teacher.TeacherId }, "New teacher has been succesfully recorded to the database!!");
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{teacherId:int}")]
        public async Task<IActionResult> DeleteTeacher(int teacherId)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(teacherId);
            
            if (teacher == null)
            {
                return NotFound();
            }

            await _unitOfWork.SchoolUsers.Remove(teacher.TeacherUserName);
            await _unitOfWork.Teachers.Remove(teacherId);
            //_context.Teachers.Remove(teacher);
            _unitOfWork.Complete();


            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.TeacherId == id)).GetValueOrDefault();
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
            foreach(char c in phoneNumber)
            {
                if(c < '0' || c > '9' || phoneNumber.Length != 10)
                    return false;
            }

            return true;
        }
    }
}
