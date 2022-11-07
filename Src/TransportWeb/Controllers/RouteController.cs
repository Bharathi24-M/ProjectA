using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Functions;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class RouteController : Controller
    {
        Routes ObjRoute = new Routes();

        public async Task<IActionResult> Index()
        {
            List<RouteInfo> RouteList = new List<RouteInfo>();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            RouteList = await ObjRoute.GetRoute();
            return View(RouteList);
        }
        public async Task<IActionResult> AddRouteDetails()
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            var RouteInfo = new RouteInfo();
            return View(RouteInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddRouteDetails(RouteInfo Route)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.AddRouteStatus = await ObjRoute.AddRoute(Route);
            }
            catch (Exception)
            {
                ViewBag.AddRouteStatus = "Something went Wrong";
            }
            return View(Route);
        }
        public async Task<IActionResult> GetRouteDetails(int id)
        {
            RouteInfo route = new RouteInfo();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            route = await ObjRoute.GetRoute(id);
            return View(route);
        }

        public async Task<IActionResult> EditRouteDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            RouteInfo Route = new RouteInfo();
            Route = await ObjRoute.GetRoute(id);
            return View(Route);
        }
        [HttpPost]
        public async Task<ActionResult> EditRouteDetails(RouteInfo Route)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.EditRouteStatus = await ObjRoute.UpdateRoute(Route);
            }
            catch (Exception)
            {
                ViewBag.EditRouteStatus = "Something went Wrong";
            }
            return View(Route);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteRouteDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            RouteInfo Route = new RouteInfo();
            Route = await ObjRoute.GetRoute(id);
            TempData["RouteNum"] = Route.RouteNum;
            return View(Route);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteRouteDetails()
        {
            RouteInfo Route = new RouteInfo();
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                int id = (int)TempData["RouteNum"];
                Route = await ObjRoute.GetRoute(id);
                ViewBag.DeleteRouteStatus = await ObjRoute.DeleteRoute(Route);
            }
            catch
            {
                ViewBag.DeleteRouteStatus = "Something went wrong";
            }
            return View(Route);
        }

    }
}
