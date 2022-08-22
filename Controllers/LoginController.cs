using DrugMMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DrugMMvc.Controllers
{
    public class LoginController : Controller
    {
        string baseURL = "https://localhost:7289/";
       

        private readonly ISession session;

        public LoginController(IHttpContextAccessor httpContextAccessor)
        {

            session = httpContextAccessor.HttpContext.Session;

        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UserLogin(User u)
        {
            HttpContext.Session.SetInt32("id", u.UserId);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                StringContent content = new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7289/api/Login/", content);

            }
            return RedirectToAction("Index", "Calling");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User u)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                StringContent content = new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7195/api/Login/", content);
            }
            return RedirectToAction("UserLogin", "Login");
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin admin)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7289/api/Login/AdminLogin", content);

            }
            return RedirectToAction("AdminIndex", "Calling");
        }
    }
}
