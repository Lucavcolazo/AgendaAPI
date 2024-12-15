using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendaAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Asignar el rol "User" al nuevo usuario
            await _userManager.AddToRoleAsync(user, "User");

            // Asignar el rol "Admin" si el nombre de usuario es "admin"
            if (model.Username == "admin")
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Ok(new { Message = "User registered successfully" });
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        return BadRequest(new { Errors = errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        if (result.Succeeded)
        {
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        if (result.IsLockedOut)
        {
            return Unauthorized(new { Message = "User account is locked out." });
        }

        if (result.IsNotAllowed)
        {
            return Unauthorized(new { Message = "User is not allowed to sign in." });
        }

        if (result.RequiresTwoFactor)
        {
            return Unauthorized(new { Message = "Two-factor authentication is required." });
        }

        return Unauthorized(new { Message = "Invalid username or password." });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        try
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception("Error generating JWT token: " + ex.Message);
        }
    }
}