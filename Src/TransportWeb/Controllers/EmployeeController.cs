using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Functions;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class EmployeeController : Controller
    {
        Employee ObjEmp = new Employee();
        Stop ObjStop = new Stop();
        Routes ObjRoute = new Routes();
        Vehicle ObjVehicle = new Vehicle();
        public async Task<IActionResult> Index()
        {
            EmployeeList EmpList = new EmployeeList();

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            EmpList.EmployeeInfoList = await ObjEmp.GetEmployee();
            EmpList.StopList = await ObjStop.GetStop();
            var routelists = await ObjRoute.GetRoute();
            EmpList.RouteList = routelists.Select(x => new SelectListItem()
            {
                Text = x.RouteName,
                Value = x.RouteNum.ToString()
            }).ToList();
            EmpList.VehicleList = await ObjVehicle.GetVehicle();
            EmpList.VehicleList = EmpList.VehicleList.Where(x => x.IsOperable == true).ToList();
            return View(EmpList);
        }
        public async Task<IActionResult> AddEmployeeDetails()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;

                return View("Error", error);
            }
            var EmpInfo = new EmployeeInfo();

            EmpInfo.RouteList = await ObjRoute.GetRoute();
            EmpInfo.StopList = await ObjStop.GetStop();
            EmpInfo.VehicleList = await ObjVehicle.GetVehicle();
            return View(EmpInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetails(EmployeeInfo EmpInfo)
        {
            try
            {

                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                ViewBag.AddEmployeestatus = await ObjEmp.AddEmployee(EmpInfo);
            }
            catch (Exception)
            {
                ViewBag.AddEmployeestatus = "Something went wrong";
                return View();
            }
            EmpInfo.RouteList = await ObjRoute.GetRoute();
            EmpInfo.StopList = await ObjStop.GetStop();
            EmpInfo.VehicleList = await ObjVehicle.GetVehicle();
            return View(EmpInfo);
        }
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            EmployeeInfo emp = await ObjEmp.GetEmployee(id);

            return View(emp);
        }
        public async Task<IActionResult> EditEmployeeDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            EmployeeInfo EmpInfo = new EmployeeInfo();

            EmpInfo = await ObjEmp.GetEmployee(id);
            EmpInfo.RouteList = await ObjRoute.GetRoute();
            EmpInfo.StopList = await ObjStop.GetStop();
            EmpInfo.VehicleList = await ObjVehicle.GetVehicle();
            TempData["OldVehicleNum"] = EmpInfo.VehicleId;
            return View(EmpInfo);
        }
        [HttpPost]
        public async Task<ActionResult> EditEmployeeDetails(EmployeeInfo EmpInfo)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                int OldVehicleNum = (int)TempData["OldVehicleNum"];
                ViewBag.EditEmployeeStatus = await ObjEmp.UpdateEmployee(EmpInfo, OldVehicleNum);
            }
            catch (Exception)
            {
                ViewBag.EditEmployeeStatus = "Something went wrong";
            }
            EmpInfo.RouteList = await ObjRoute.GetRoute();
            EmpInfo.StopList = await ObjStop.GetStop();
            EmpInfo.VehicleList = await ObjVehicle.GetVehicle();

            return View(EmpInfo);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteEmployeeDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            EmployeeInfo EmpInfo = new EmployeeInfo();
            EmpInfo = await ObjEmp.GetEmployee(id);
            TempData["EmployeeId"]=EmpInfo.EmployeeId;
            EmpInfo.RouteList = await ObjRoute.GetRoute();
            EmpInfo.StopList = await ObjStop.GetStop();
            EmpInfo.VehicleList = await ObjVehicle.GetVehicle();
            EmpInfo.VehicleList = EmpInfo.VehicleList.Where(x => x.IsOperable).ToList();           
            return View(EmpInfo);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteEmployeeDetails()
        {
            EmployeeInfo EmpInfo = new EmployeeInfo();
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }

                int EmployeeId = (int)TempData["EmployeeId"];
                if (EmpInfo == null)
                {
                    EmpInfo = new EmployeeInfo();
                }
                EmpInfo = await ObjEmp.GetEmployee(EmployeeId);
                ViewBag.DeleteEmployeeStatus = await ObjEmp.DeleteEmployee(EmpInfo);
                EmpInfo.RouteList = await ObjRoute.GetRoute();
                EmpInfo.StopList = await ObjStop.GetStop();
                EmpInfo.VehicleList = await ObjVehicle.GetVehicle();
            }
            catch
            {
                ViewBag.DeleteEmployeeStatus = "Something went wrong";
            }
            return View(EmpInfo);
        }

    }
}