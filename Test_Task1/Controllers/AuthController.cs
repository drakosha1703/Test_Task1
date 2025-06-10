using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_task.models;

namespace test_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var account = await _context.Accounts
                .Include(a => a.Founders)
                .FirstOrDefaultAsync(a => a.User == request.User && a.Password == request.Password);

            if (account == null)
                return Unauthorized("Неверный логин или пароль");

            return Ok(new
            {
                AccountId = account.Id,
                FounderId = account.Founders.Id,
                FounderFullName = account.Founders.FullName,
                Message = "Успешный вход"
            });
        }
    }

    public class LoginRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}