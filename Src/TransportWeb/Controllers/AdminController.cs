using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class AdminController : Controller
    {
        public static List<AdminInfo> Logininfo = new List<AdminInfo>();
        private readonly ISession session;
        public AdminController(IHttpContextAccessor httpContextAccessor)
        {

            session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminInfo AInfo)
        {
            using (var client = new HttpClient())
            {
                var Loginobj = new AdminInfo();
                StringContent content = new StringContent(JsonConvert.SerializeObject(AInfo), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7291/api/TransportApi", content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        Loginobj = JsonConvert.DeserializeObject<AdminInfo>(apiresponse);
                        HttpContext.Session.SetString("UserName", apiresponse);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ViewBag.LoginError = "Unable to login...! Please try again";
                        return View();
                    }
                }
            }

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                TempData["Logoutstatus"] = "Logged out Successfully...";
            }
            return RedirectToAction("Login");
        }
        public IActionResult Error()
        {
            var error = new ErrorViewModel();
            error.Errorcode = 404;
            return View(error);
        }
    }
}

