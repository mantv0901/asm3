using Estore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Repository;
using BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Estore.Controllers
{
    public class AdminAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberRepository _memberRepository;
        private readonly IConfiguration config;
        public HomeController(ILogger<HomeController> logger, IMemberRepository memberRepository,IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            config = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> login(string email, string password)
        {
            AdminAccount admin = new AdminAccount()
            {
                Email = "admin@fstore.com",
                Password = "admin@@"
            };
            string a = config.GetConnectionString("Conn");
            string Email= null, role=null;
            Member mem = new Member();
            if (email == admin.Email && password == admin.Password)
            {
                Email = email;
                role = "Admin";
            } else if ((mem = await _memberRepository.Get(x => x.Email == email && x.Password == password && x.Status ==true)) != null)
            {
                Email = mem.Email;
                role = "Member";
            }
            if (Email == null)
            {
                ViewData["Mess"] = "Wrong Email or Password";
                return View("Index");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", Email));
            claims.Add(new Claim(ClaimTypes.Role, role));
            HttpContext.Session.SetString("USER", JsonConvert.SerializeObject(mem));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            if (role == "Admin") return RedirectToAction("Index", "Products");
            else return View("Member");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
