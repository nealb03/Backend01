using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Models;           // Namespace where Cloud495Context and User live
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Cloud495Context _context;

        public AuthController(Cloud495Context context)
        {
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // basic request validation
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            // find matching user (case-insensitive email)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            //------------------------------------------------------------
            // compute one or more possible password hashes
            //------------------------------------------------------------
            string ComputeSha256(string input)
            {
                using var sha = SHA256.Create();
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }

            var trimmed = request.Password.Trim();

            // standard SHA-256 of the input
            string hashNormal = ComputeSha256(trimmed);
            // lowercase variant
            string hashLowercaseInput = ComputeSha256(trimmed.ToLowerInvariant());
            // double-hashed variant (SHA256(SHA256(password)))
            string hashDouble = ComputeSha256(hashNormal);

            //------------------------------------------------------------
            // compare against stored value, permitting all variant hashes
            //------------------------------------------------------------
            var stored = user.Password?.Trim() ?? string.Empty;
            bool match =
                string.Equals(stored, hashNormal, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(stored, hashLowercaseInput, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(stored, hashDouble, StringComparison.OrdinalIgnoreCase);

            if (!match)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            //------------------------------------------------------------
            // success response
            //------------------------------------------------------------
            return Ok(new
            {
                message = "Login successful.",
                user = new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email
                }
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}