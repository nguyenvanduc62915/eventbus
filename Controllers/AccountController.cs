using CloneControllerAccount.Dtos;
using CloneControllerAccount.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CloneControllerAccount.EventBusConfig;
using System;
using System.Threading.Tasks;

namespace CloneControllerAccount.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountService;
        private readonly EventBus _eventBus;

        public AccountController(IAccountServices accountService, EventBus eventBus)
        {
            _accountService = accountService;
            _eventBus = eventBus;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUsersAsync(registerDto);
                if (result.Succeeded)
                {
                    var userRegisteredEvent = new UserRegisteredEvent
                    {
                        UserName = registerDto.Email,
                        Email = registerDto.Email,
                        EventTime = DateTime.UtcNow
                    };
                    await _eventBus.Publish(userRegisteredEvent);

                    return RedirectToAction("Login");
                }
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
            return View(registerDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var token = await _accountService.LoginUsersAsync(loginDto);
                if (token != null)
                {
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SameSite = SameSiteMode.Strict
                    });

                    var userLoggedInEvent = new UserLoggedInEvent
                    {
                        UserName = loginDto.Email,
                        Token = token,
                        EventTime = DateTime.UtcNow
                    };
                    await _eventBus.Publish(userLoggedInEvent);

                    return RedirectToAction("OkLike");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginDto);
        }

        [Authorize(Roles = "User")]
        public IActionResult OkLike()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated;
            var userName = User.Identity?.Name ?? "Guest";
            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.UserName = userName;
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
}
