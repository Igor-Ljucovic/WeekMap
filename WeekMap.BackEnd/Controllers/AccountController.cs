using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (_context.Users.Any(u => u.Username == user.Username))
            return BadRequest(new { field = "username", message = "Username already exists." });

        if (_context.Users.Any(u => u.Email.ToLower() == user.Email.ToLower()))
            return BadRequest(new { field = "email", message = "Email is already in use." });

        user.EmailConfirmationToken = GenerateSecureToken(32);
        user.EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
        user.IsEmailConfirmed = false;

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        _context.SaveChanges();

        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = user.EmailConfirmationToken, userId = user.UserID }, protocol: Request.Scheme);
        new EmailConfirmationService().SendConfirmationEmail(user, confirmationLink);

        return Ok(new { message = "User registered successfully. Check your email to confirm." });
    }

    private string GenerateSecureToken(int byteLength = 32)
    {
        byte[] tokenBytes = new byte[byteLength];
        RandomNumberGenerator.Fill(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        if (!user.IsEmailConfirmed)
        {
            ModelState.AddModelError("", "Please confirm your email before logging in.");
            return View();
        }

        HttpContext.Session.SetInt32("UserID", user.UserID);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ConfirmEmail(string token, int userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserID == userId && 
                                                 u.EmailConfirmationToken == token &&
                                                 u.EmailConfirmationTokenExpiresAt > DateTime.UtcNow);

        if (user.EmailConfirmationTokenExpiresAt < DateTime.UtcNow)
        {
            TempData["Message"] = "This confirmation link is invalid or expired.";
            return RedirectToAction("Login");
        }

        if (user == null)
        {
            return NotFound();
        }

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailConfirmationTokenExpiresAt = null;
        _context.SaveChanges();

        TempData["Message"] = "Your email has been verified!";
        return RedirectToAction("Login");
    }
}
