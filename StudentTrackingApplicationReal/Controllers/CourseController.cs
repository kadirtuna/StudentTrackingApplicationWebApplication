using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationBackEnd.Services;
using StudentTrackingApplicationBackEnd.DTO;
using StudentTrackingApplication.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private Context _context;
        private readonly ILogger<CourseController> _logger;
        private IUnitOfWork _unitOfWork;

        public CourseController(Context context, ILogger<CourseController> logger)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = new UnitOfWork(context);
        }

        // GET: api/Course
        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<IEnumerable<ReadCourseDto>>> GetCourses()
        {
            List<ReadCourseDto> listReadCourseDto = new List<ReadCourseDto>();

            foreach (Course course in await _unitOfWork.Courses.GetAll())
            {
                ReadCourseDto readCourseDto = new ReadCourseDto()
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseCredits = course.CourseCredits,
                    CourseNumbers = course.CourseNumbers,
                    TeacherId = course.TeacherId,
                    CourseTotalPrice = course.CourseTotalPrice,
                    CourseCreatedDate = course.CourseCreatedDate,
                    CourseStartDate = course.CourseStartDate,
                    CourseDays = course.CourseDays,
                };

                listReadCourseDto.Add(readCourseDto);
            }

            return Ok(listReadCourseDto);
        }
        // GET: api/Course/5
        [HttpGet("{courseId:int}/")]
        [Authorize(Roles = "Admin, Manager, Teacher")]
        public async Task<ActionResult<ReadCourseDto>> GetCourse(int courseId)
        {
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;
            var course = await _unitOfWork.Courses.Get(courseId);

            if (course == null)
            {
                return NotFound();
            }

            var teacher = await _unitOfWork.Teachers.Get(course.TeacherId);

            if (teacher.TeacherUserName != userName && userRole == "Teacher")
            {
                return BadRequest("You are not allowed for this query due to you aren't the teacher of given course!");
            }

            ReadCourseDto readCourseDto = new ReadCourseDto()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseCredits = course.CourseCredits,
                CourseNumbers = course.CourseNumbers,
                TeacherId = course.TeacherId,
                CourseTotalPrice = course.CourseTotalPrice,
                CourseCreatedDate = course.CourseCreatedDate,
                CourseStartDate = course.CourseStartDate,
                CourseDays = course.CourseDays,
            };

            return readCourseDto;
        }

        [HttpGet("CoursesPrices")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReadCoursesPricesDto>>> GetCoursesPrices()
        {
            List<ReadCoursesPricesDto> readCoursesPricesDto = new List<ReadCoursesPricesDto>();

            foreach (Course course in await _unitOfWork.Courses.GetAll())
            {
                readCoursesPricesDto.Add(new ReadCoursesPricesDto()
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseTotalPrice = course.CourseTotalPrice,
                });
            }

            if (readCoursesPricesDto == null)
            {
                return NotFound("There isn't any course record in Courses!");
            }

            return readCoursesPricesDto;
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{courseId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PutCourse(int courseId, UpdateCourseDto course)
        {
            Course originalCourse = await _unitOfWork.Courses.Get(courseId);

            if (originalCourse == null)
            {
                return BadRequest("This Course couldn't be found with given courseId!");
            }

            originalCourse.CourseName = course.CourseName;
            originalCourse.CourseCredits = course.CourseCredits;
            originalCourse.CourseNumbers = course.CourseNumbers;
            originalCourse.TeacherId = course.TeacherId;
            originalCourse.CourseTotalPrice = course.CourseTotalPrice;
            originalCourse.CourseDays = course.CourseDays;
            originalCourse.TimeFirst2Digits = course.TimeFirst2Digits;
            originalCourse.TimeLast2Digits = course.TimeLast2Digits;
            

            //                public string CourseName { get; set; } = null;
            //    public int CourseCredits { get; set; } = 0;
            //    public int CourseNumbers { get; set; } = 0;
            //    public int TeacherId { get; set; } = 0;
            //    public int CourseTotalPrice { get; set; } = 0;
            //    public int CourseDays { get; set; } = 0;//The guideline about CourseDays is in the comment block below.
            //    public int TimeFirst2Digits { get; set; } = 0;
            //    public int TimeLast2Digits { get; set; } = 0;
            //}

            //_unitOfWork.Courses.Update(course);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(courseId))
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

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Course>> PostCourse(CreateCourseDto request)
        {
            List<int> ListCourseDays = CreateCourseDto.DayAdjuster(request.CourseDays);

            if (ListCourseDays.ElementAt(0) == 0)
                return BadRequest("CourseDays range has to be between 0 inclusive to 15 inclusive!");

            var course = new Course()
            {
                CourseId = 0,
                CourseName = request.CourseName,
                CourseCredits = request.CourseCredits,
                CourseNumbers = request.CourseNumbers,
                TeacherId = request.TeacherId,
                CourseTotalPrice = request.CourseTotalPrice,
                CourseCreatedDate = DateTime.Now,
                CourseStartDate = new DateTime(2022, 09, 19, request.TimeFirst2Digits, request.TimeLast2Digits, 0),
                CourseDays = request.CourseDays,
            };


            await _unitOfWork.Courses.Add(course);
            _unitOfWork.Complete();

            ReadCourseDto readCourseDto = new ReadCourseDto()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseCredits = course.CourseCredits,
                CourseNumbers = course.CourseNumbers,
                TeacherId = course.TeacherId,
                CourseTotalPrice = course.CourseTotalPrice,
                CourseCreatedDate = course.CourseCreatedDate,
                CourseStartDate = course.CourseStartDate,
                CourseDays = course.CourseDays,
            };

            return CreatedAtAction("GetCourse", new { courseId = course.CourseId }, "The new course has been successfully recorded to the database!");
        }

        // DELETE: api/Course/5
        [HttpDelete("{courseId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var course = await _unitOfWork.Courses.Get(courseId);

            if (course == null)
            {
                return NotFound();
            }

            _unitOfWork.Courses.Remove(courseId);
            _unitOfWork.Complete();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}