using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Functions;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class StopController : Controller
    {
        Stop ObjStop = new Stop();
        Routes ObjRoute = new Routes();
        public async Task<IActionResult> Index()
        {
            var StopList = new StopList();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            StopList.StopInfoList = await ObjStop.GetStop();
            var routelists = await ObjRoute.GetRoute();
            StopList.RouteList = routelists.Select(x => new SelectListItem()
            {
                Text = x.RouteName,
                Value = x.RouteNum.ToString()
            }).ToList();
            return View(StopList);
        }
        public async Task<IActionResult> AddStopDetails()
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            var StopInfo = new StopInfo();
            var routelists = await ObjRoute.GetRoute();
            StopInfo.RouteList = routelists.Select(x => new SelectListItem()
            {
                Text = x.RouteName,
                Value = x.RouteNum.ToString()
            }).ToList();
            return View(StopInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddStopDetails(StopInfo stop)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.AddStopStatus = await ObjStop.AddStop(stop);
                var routelists = await ObjRoute.GetRoute();
                stop.RouteList = routelists.Select(x => new SelectListItem()
                {
                    Text = x.RouteName,
                    Value = x.RouteNum.ToString()
                }).ToList();
            }
            catch (Exception)
            {
                ViewBag.AddStopStatus = "Something went Wrong";
            }
            return View(stop);
        }
        public async Task<IActionResult> GetStopDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            var Stop = await ObjStop.GetStop(id);
            return View(Stop);
        }

        public async Task<IActionResult> EditStopDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            var Stop = await ObjStop.GetStop(id);
            var routelists = await ObjRoute.GetRoute();
            Stop.RouteList = routelists.Select(x => new SelectListItem()
            {
                Text = x.RouteName,
                Value = x.RouteNum.ToString()
            }).ToList();
            return View(Stop);
        }

        [HttpPost]
        public async Task<ActionResult> EditStopDetails(StopInfo Sobj)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.EditStopStatus = await ObjStop.UpdateStop(Sobj);
                var routelists = await ObjRoute.GetRoute();
                Sobj.RouteList = routelists.Select(x => new SelectListItem()
                {
                    Text = x.RouteName,
                    Value = x.RouteNum.ToString()
                }).ToList();
            }
            catch (Exception)
            {
                ViewBag.EditStopStatus = "Something went wrong!";
            }
            return View(Sobj);
        }
        public async Task<ActionResult> DeleteStopDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            var Stop = await ObjStop.GetStop(id);
            var routelists = await ObjRoute.GetRoute();
            Stop.RouteList = routelists.Select(x => new SelectListItem()
            {
                Text = x.RouteName,
                Value = x.RouteNum.ToString()
            }).ToList();
            TempData["StopId"] = Stop.StopId;
            return View(Stop);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteStopDetails()
        {
            StopInfo StopInfo = new StopInfo();
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
               
                var id = (int)TempData["StopId"];
                StopInfo = await ObjStop.GetStop(id);
                ViewBag.DeleteStopStatus = await ObjStop.DeleteStop(StopInfo);
                var routelists = await ObjRoute.GetRoute();
                StopInfo.RouteList = routelists.Select(x => new SelectListItem()
                {
                    Text = x.RouteName,
                    Value = x.RouteNum.ToString()
                }).ToList();

            }
            catch
            {
                ViewBag.DeleteStopStatus = "Something went wrong";
            }
            return View(StopInfo);

            //return RedirectToAction("Index");
        }

    }
}
