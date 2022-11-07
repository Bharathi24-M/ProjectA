using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Functions;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class VehicleController : Controller
    {
        Routes ObjRoute = new Routes();
        Vehicle ObjVehicle = new Vehicle();
        Employee ObjEmp = new Employee();
        public async Task<IActionResult> Index()
        {
            Vehicles Vehicle = new Vehicles();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            Vehicle.VehicleList = await ObjVehicle.GetVehicle();
            Vehicle.RouteList = await ObjRoute.GetRoute();
            return View(Vehicle);
        }
        public async Task<IActionResult> AddVehicleDetails()
        {
            VehicleInfo Vehicle = new VehicleInfo();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            Vehicle.RouteInfos = await ObjRoute.GetRoute();
            return View(Vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleDetails(VehicleInfo Vehicle)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.AddVehiclestatus = await ObjVehicle.AddVehicle(Vehicle);
                Vehicle.RouteInfos = await ObjRoute.GetRoute();
            }
            catch (Exception)
            {
                ViewBag.AddVehiclestatus = "Something Went Wrong";
                return View();
            }

            return View(Vehicle);
        }
        public async Task<IActionResult> GetVehicleDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            VehicleInfo Vehicle = new VehicleInfo();
            Vehicle = await ObjVehicle.GetVehicle(id);
            return View(Vehicle);
        }

        public async Task<IActionResult> EditVehicleDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            VehicleInfo Vehicle = new VehicleInfo();
            Vehicle = await ObjVehicle.GetVehicle(id);
            Vehicle.RouteInfos = await ObjRoute.GetRoute();
            TempData["OldRouteNum"] = Vehicle.RouteNum;
            return View(Vehicle);
        }
        [HttpPost]
        public async Task<ActionResult> EditVehicleDetails(VehicleInfo Vehicle)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                Vehicle.RouteInfos = await ObjRoute.GetRoute();
                if ((int)TempData["OldRouteNum"] != Vehicle.RouteNum)
                {
                    var emplist = await ObjEmp.GetEmployee();
                    var count = emplist.Where(x => x.VehicleId == Vehicle.VehicleId).Count();
                    if (count > 0)
                    {
                        ViewBag.EditVehicleStatus = "Unable to change the route name, Vehicle already assigned to the Employee";
                        return View(Vehicle);
                    }
                }
                ViewBag.EditVehicleStatus = await ObjVehicle.UpdateVehicle(Vehicle);

            }
            catch (Exception)
            {
                ViewBag.EditVehicleStatus = "Something went wrong";
            }

            return View(Vehicle);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteVehicleDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            VehicleInfo Vehicle = new VehicleInfo();
            Vehicle = await ObjVehicle.GetVehicle(id);
            TempData["VehicleId"] = Vehicle.VehicleId;
            return View(Vehicle);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteVehicleDetails()
        {
            VehicleInfo Vehicle = new VehicleInfo();
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                var VehicleId = (int)TempData["VehicleId"];
                Vehicle = await ObjVehicle.GetVehicle(VehicleId);


                var emplist = await ObjEmp.GetEmployee();
                var count = emplist.Where(x => x.VehicleId == Vehicle.VehicleId).Count();
                if (count > 0)
                {
                    ViewBag.DeleteVehicleStatus = "Unable to delete, Vehicle already assigned to the Employee";
                    return View(Vehicle);
                }
                ViewBag.DeleteVehicleStatus = await ObjVehicle.DeleteVehicle(Vehicle);
            }
            catch (Exception)
            {
                ViewBag.DeleteVehicleStatus = "Something went wrong";
            }
            return View(Vehicle);
        }
    }
}
