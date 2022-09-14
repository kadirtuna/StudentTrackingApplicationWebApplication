using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.DTO;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using StudentTrackingApplicationBackEnd.Services;
using StudentTrackingApplicationBackEnd.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using StudentTrackingApplication.Shared.DTO;

namespace StudentTrackingApplicationBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private Context _context;
        private readonly ILogger<AuthController> _logger;
        private readonly ILogger<CourseController> _logger1;
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public AuthController(Context context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = new UnitOfWork(context);
        }

        [HttpGet("UserList")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadSchoolUserDto>> GetUserList()
        {
            List<ReadSchoolUserDto> userList = new List<ReadSchoolUserDto>();
            
            foreach (SchoolUser user in await _unitOfWork.SchoolUsers.GetAll())
            {   
                ReadSchoolUserDto readUserDto = new ReadSchoolUserDto();
                readUserDto.UserName = user.UserName;
                readUserDto.UserRole = user.UserRole;
                userList.Add(readUserDto);
            }

            return Ok(userList);
        }

        [HttpGet("UserInformation")]
        [Authorize]
        public async Task<ActionResult<SchoolUser>> GetUserInformation()
        {
            string userName = User.FindFirst(ClaimTypes.Name).Value;
            string userRole = User.FindFirst(ClaimTypes.Role).Value;
            Manager manager = null;
            Teacher teacher = null;
            Student student = null;

            if (userRole == "Manager")
            {
                foreach (Manager currentManager in await _unitOfWork.Managers.GetAll())
                {
                    if (currentManager.ManagerUserName== userName)
                        manager = currentManager;
                }
            }
            else if (userRole == "Teacher")
            {
                foreach(Teacher currentTeacher in await _unitOfWork.Teachers.GetAll())
                {
                    if (currentTeacher.TeacherUserName == userName)
                        teacher = currentTeacher;
                }
            }
            else if(userRole == "Student")
            {
                foreach (Student currentStudent in await _unitOfWork.Students.GetAll())
                {
                    if (currentStudent.StudentUserName == userName)
                        student = currentStudent;
                }
            }


            try
            {
                if(teacher != null)
                    return Ok(teacher);
                else
                {
                    throw new NullReferenceException();
                }
            }catch(NullReferenceException error1)
            {
                try
                {
                    if(student != null)
                        return Ok(student);
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                catch (NullReferenceException error)
                {
                    try
                    {
                        if (manager != null)
                            return Ok(manager);
                        else
                        {
                            throw new NullReferenceException();
                        }
                    }
                    catch(NullReferenceException)
                    {
                        SchoolUser schoolUser = await _unitOfWork.SchoolUsers.Get(userName);
                        ReadSchoolUserDto readSchoolUserDto = new ReadSchoolUserDto()
                        {
                            UserName = schoolUser.UserName,
                            UserRole = schoolUser.UserRole
                        };

                        return Ok(readSchoolUserDto);
                    }
                }
            }
            return BadRequest("This user couldn't be found on the database!");
        }

        [HttpPost("Register")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<SchoolUser>> Register(CreateSchoolUserDto request)
        {
            SchoolUser existingUserControl = await _unitOfWork.SchoolUsers.Get(request.UserName);
            if (existingUserControl != null)
                return BadRequest("There is an alredy account with given UserName!");

            switch (request.UserRole)
            {
                case "Admin":
                    break;
                case "Manager":
                    break;
                case "Teacher":
                    break;
                case "Student":
                    break;
                default:
                    return BadRequest("The User Role can just be \"Admin\", \"Manager\", \"Teacher\" or \"Student\"!");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            SchoolUser user = new SchoolUser()
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserRole = request.UserRole
            };

            await _unitOfWork.SchoolUsers.Add(user);
            _unitOfWork.Complete();

            return Ok(user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<SchoolUser>> Login(UserDto request)
        {
            SchoolUser requestedUser = await _unitOfWork.SchoolUsers.Get(request.UserName);
            CreatedTokenDto createdTokenDto = new CreatedTokenDto
            {
                success = true,
            };

            if (requestedUser == null)
            {
                createdTokenDto.token = "1";
                createdTokenDto.success = false;
                return BadRequest(createdTokenDto);
            }

            if (!VerifyPasswordHash(request.Password, requestedUser.PasswordHash, requestedUser.PasswordSalt))
            {
                createdTokenDto.token = "2";
                createdTokenDto.success = false;
                return BadRequest(createdTokenDto);
            }
                
            string token = CreateToken(requestedUser);
            createdTokenDto.token = token;
            _unitOfWork.Complete();

            return Ok(createdTokenDto);
        }

        [HttpDelete("{userName}/{password}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SchoolUser>> DeleteUser(string userName, string password)
        {
            SchoolUser requestedUser = await _unitOfWork.SchoolUsers.Get(userName);
            if(requestedUser.UserRole == "Manager" || requestedUser.UserRole == "Teacher" || requestedUser.UserRole == "Student")
                return BadRequest("A Manager, a teacher or a student can just be deleted on their own Controller Page!");

            if (requestedUser == null)
                return BadRequest("Given username couldn't be found!");

            if (!VerifyPasswordHash(password, requestedUser.PasswordHash, requestedUser.PasswordSalt));
                return BadRequest("Given password is wrong for given username!");

            _unitOfWork.SchoolUsers.Remove(userName);
            return NoContent();
        }

        private string CreateToken(SchoolUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }
        private void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }
        
        private bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}
