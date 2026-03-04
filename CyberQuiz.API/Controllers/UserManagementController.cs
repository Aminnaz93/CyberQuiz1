using CyberQuiz.BLL.Services;
using CyberQuiz.DAL.Models;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserManagementController : ControllerBase
    {

        // 1. UserService — hanterar inloggning och registrering
        private readonly IUserService _userService;
        // 2. UserManager — hämtar användare från databasen
        private readonly UserManager<ApplicationUser> _userManager;
        // 3. Configuration — läser JWT-nyckeln från appsettings.json
        private readonly IConfiguration _configuration;

        public UserManagementController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
        }


        // POST api/auth/register
        // UI skickar email + lösenord hit när någon registrerar sig
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Kolla att email och lösenord är ifyllda korrekt
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Försök skapa användaren i databasen
            var result = await _userService.CreateUserAsync(dto.Email, dto.Password);

            // Om det gick fel (t.ex. lösenordet för kort) — skicka tillbaka felen
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return BadRequest(ModelState);
            }

            // Hämta den nyskapade användaren från databasen
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return StatusCode(500, "Något gick fel.");

            // Skapa en token och skicka tillbaka den till UI
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }


        // POST api/auth/login
        // UI skickar email och lösenord hit när någon loggar in

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Kolla om email och lösenord är ifyllda korrekt
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kontrollera att email och lösenord är rätt
            var result = await _userService.PasswordSignInAsync(
                dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);

            // Om fel lösenord eller email — skicka tillbaka felmeddelande
            if (!result.Succeeded)
                return Unauthorized("Fel e-post eller lösenord.");

            // Hämta användaren från databasen
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized();

            // Skapa en token och skicka tillbaka den till UI
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }



        // GET api/auth/me
        // UI anropar denna för att kolla vem som är inloggad
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Plocka ut användarens id från token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Hämta användaren från databasen med det id:t
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            // Skicka tillbaka användarens info till UI
            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty
            });
        }


        // HJÄLPMETOD — skapar en JWT-token för en användare
        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT-nyckel saknas i appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

//using Microsoft.AspNetCore.Identity;

//namespace CyberQuiz.API.Controllers
//{


//    public class UserManagementController
//    {
//        private readonly SignInManager signInManager;

//        public UserManagementController(SignInManager SignInManager)
//        {
//            signInManager = SignInManager;
//        }
//    }
//}
