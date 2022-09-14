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
    [Authorize(Roles = "Admin")]
    public class ManagerController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<ManagerController> _logger;
        private readonly ILogger<AuthController> _logger1;
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public ManagerController(Context context, IConfiguration configuration, ILogger<ManagerController> logger)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _unitOfWork = new UnitOfWork(_context);
        }

        // GET: api/Manager
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManager()
        {
          if (_context.Manager == null)
          {
              return NotFound();
          }
            return await _context.Manager.ToListAsync();
        }

        // GET: api/Manager/5
        [HttpGet("{managerId:int}")]
        public async Task<ActionResult<Manager>> GetManager(int managerId)
        {
          if (_context.Manager == null)
          {
              return NotFound();
          }
            var manager = await _context.Manager.FindAsync(managerId);

            if (manager == null)
            {
                return NotFound();
            }

            return manager;
        }

        // PUT: api/Manager/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{managerId:int}")]
        public async Task<IActionResult> PutManager(int managerId, CreateManagerDto manager)
        {
            if (IsDigitsOnly(manager.ManagerPhone) == false)
            {
                return BadRequest("Phone Number can have just 0-9 numbers and can just have 10 digits length!");
            }

            Manager originalManager = await _unitOfWork.Managers.Get(managerId);

            if (originalManager == null)
            {
                return BadRequest("This Manager couldn't be found with given ManagerId!");
            }

            originalManager.ManagerName = manager.ManagerName;
            originalManager.ManagerSurname = manager.ManagerSurname;
            originalManager.ManagerEmail = manager.ManagerEmail;
            originalManager.ManagerPhone = manager.ManagerPhone;
            _context.Entry(originalManager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(managerId))
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

        // POST: api/Manager
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Manager>> PostManager(CreateManagerDto createManagerDto)
        {
            if (IsDigitsOnly(createManagerDto.ManagerPhone) == false)
            {
                return BadRequest("Phone Number can have just 0-9 numbers and can just have 10 digits length!");
            }

            

            string managerUserName = Convert.ToString(createManagerDto.ManagerName[0]);
            managerUserName += createManagerDto.ManagerSurname;
            managerUserName = managerUserName.ToLower();
            managerUserName = TurkishCharacterToEnglish(managerUserName);
            int i = 0;

            while (true)
            {
                i++;
                SchoolUser existingUserControl = await _unitOfWork.SchoolUsers.Get(managerUserName);
                if (existingUserControl != null)
                {
                    if (i > 1)
                        managerUserName.Remove(managerUserName.Length - 1, 1);
                    managerUserName += Convert.ToString(i);
                }
                else
                    break;
            }
            Manager manager = new Manager()
            {
                ManagerName = createManagerDto.ManagerName,
                ManagerSurname = createManagerDto.ManagerSurname,
                ManagerEmail = createManagerDto.ManagerEmail,
                ManagerPhone = createManagerDto.ManagerPhone,
                ManagerUserName = managerUserName,
                ManagerCreatedDate = DateTime.Now
            };

            CreateSchoolUserDto createUserDto = new CreateSchoolUserDto()
            {
                UserName = managerUserName,
                Password = managerUserName,
                UserRole = "Manager",
            };

            await _unitOfWork.Managers.Add(manager);
            var result = await new AuthController(_context, _configuration, _logger1).Register(createUserDto);
            _unitOfWork.Complete();

            return CreatedAtAction("GetManager", new { managerId = manager.ManagerId }, "The new Manager has been successfully recorded to the database!");
        }

        // DELETE: api/Manager/5
        [HttpDelete("{managerId:int}")]
        public async Task<IActionResult> DeleteManager(int managerId)
        {
            if (_context.Manager == null)
            {
                return NotFound();
            }
            var manager = await _context.Manager.FindAsync(managerId);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Manager.Remove(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManagerExists(int managerId)
        {
            return (_context.Manager?.Any(e => e.ManagerId == managerId)).GetValueOrDefault();
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
