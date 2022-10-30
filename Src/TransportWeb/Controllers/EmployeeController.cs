using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class EmployeeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            Employee EmpList = new Employee();

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Employee");
                if (Res.IsSuccessStatusCode)
                {
                    var empress = Res.Content.ReadAsStringAsync().Result;
                    EmpList.EmployeeInfoList = JsonConvert.DeserializeObject<List<EmployeeInfo>>(empress);//to convert json to list

                }
            }
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Stop");
                if (Res.IsSuccessStatusCode)
                {
                    var stopjson = Res.Content.ReadAsStringAsync().Result;
                    EmpList.StopList = JsonConvert.DeserializeObject<List<StopInfo>>(stopjson).ToList();
                }

            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var Routelist = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse);
                    EmpList.RouteList = Routelist.Select(x => new SelectListItem()
                    {
                        Text = x.RouteName,
                        Value = x.RouteNum.ToString()
                    }).ToList();
                }
            }
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
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var Routelist = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse);
                    EmpInfo.RouteList = Routelist.ToList();
                }
            }
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Stop");
                if (Res.IsSuccessStatusCode)
                {
                    var stopjson = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo.StopList = JsonConvert.DeserializeObject<List<StopInfo>>(stopjson).ToList();
                }
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Vehicle");
                if (Res.IsSuccessStatusCode)
                {
                    var VehicleJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var VehicleList = JsonConvert.DeserializeObject<List<VehicleInfo>>(VehicleJsonResponse);
                    EmpInfo.VehicleList = VehicleList.Where(x => x.IsOperable).ToList();
                }
            }

            return View(EmpInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetails(EmployeeInfo e)
        {
            var empobj = new EmployeeInfo();
            try
            {

                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                using (var client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("https://localhost:7291/api/Employee", content))
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        empobj = JsonConvert.DeserializeObject<EmployeeInfo>(apiresponse);
                        if (response.IsSuccessStatusCode)
                        {
                            await SetSeatCount(e.VehicleNum, "DEC");
                            ViewBag.AddEmployeestatus = "Employee details Successfully added..";
                        }
                        else
                        {
                            ViewBag.AddEmployeestatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AddEmployeestatus = "Something went wrong";
                return View();
            }
            empobj.RouteList = new List<RouteInfo>();
            empobj.VehicleList = new List<VehicleInfo>();
            empobj.StopList = new List<StopInfo>();
            // return RedirectToAction("Index");
            return View(empobj);
        }
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            EmployeeInfo emp = new EmployeeInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Employee/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                }

            }
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
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Employee/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    EmpInfo = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                    TempData["OldVehicleNum"] = EmpInfo.VehicleNum;

                }
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var Routelist = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse);
                    EmpInfo.RouteList = Routelist.ToList();
                }
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Stop");
                if (Res.IsSuccessStatusCode)
                {
                    var empress = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo.StopList = JsonConvert.DeserializeObject<List<StopInfo>>(empress).ToList();
                }
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Vehicle");
                if (Res.IsSuccessStatusCode)
                {
                    var VehicleJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var VehicleList = JsonConvert.DeserializeObject<List<VehicleInfo>>(VehicleJsonResponse);
                    EmpInfo.VehicleList = VehicleList.Where(x => EmpInfo.VehicleNum == x.VehicleNum || x.IsOperable).ToList();
                }
            }

            return View(EmpInfo);
        }
        [HttpPost]
        public async Task<ActionResult> EditEmployeeDetails(EmployeeInfo e)
        {
            EmployeeInfo receivedemp = new EmployeeInfo();
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                using (var httpClient = new HttpClient())
                {

                    int id = e.EmployeeId;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7291/api/Employee/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedemp = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            //  await CheckAvailability(e.VehicleNum);

                            if (e.VehicleNum != TempData["OldVehicleNum"])
                            {
                                await SetSeatCount(e.VehicleNum, "DEC");
                                await SetSeatCount((string)TempData["OldVehicleNum"], "INC");

                            }
                            ViewBag.EditEmployeeStatus = "Employee details updated successfully";
                        }
                        else
                        {
                            ViewBag.EditEmployeeStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.EditEmployeeStatus = "Something went wrong";
            }
            receivedemp.RouteList = new List<RouteInfo>();
            receivedemp.VehicleList = new List<VehicleInfo>();
            receivedemp.StopList = new List<StopInfo>();

            return View(receivedemp);
            //return RedirectToAction("Index");
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
            EmployeeInfo emp = new EmployeeInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Employee/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TempData["EmployeeId"] = apiResponse;
                    emp = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                }
            }
            return View(emp);
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

                EmpInfo = JsonConvert.DeserializeObject<EmployeeInfo>(TempData["EmployeeId"].ToString());

                if (EmpInfo == null)
                {
                    EmpInfo = new EmployeeInfo();
                }

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Employee/" + EmpInfo.EmployeeId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            await SetSeatCount(EmpInfo.VehicleNum, "INC");
                            ViewBag.DeleteEmployeeStatus = "Employee details deleted successfully";
                        }
                        else
                        {
                            ViewBag.DeleteEmployeeStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch
            {
                ViewBag.DeleteEmployeeStatus = "Something went wrong";
            }
            return View(EmpInfo);
            //return RedirectToAction("Index");
        }
        private async Task<VehicleInfo> SetSeatCount(string VehicleNum, string type)
        {
            var Vehicle = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var Vehicleresponse = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + VehicleNum))
                {
                    string apiResponse = await Vehicleresponse.Content.ReadAsStringAsync();
                    Vehicle = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                    if (type == "INC")
                    {
                        Vehicle.AvailableSeats = Vehicle.AvailableSeats + 1;
                    }
                    else
                    {
                        Vehicle.AvailableSeats = Vehicle.AvailableSeats - 1;

                    }
                    if (Vehicle.AvailableSeats <= 0)
                    {
                        Vehicle.AvailableSeats = 0;
                        Vehicle.IsOperable = false;
                    }
                    else
                    {
                        Vehicle.IsOperable = true;
                    }
                }
            }
            using (var httpClient = new HttpClient())
            {

                string id = VehicleNum;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(Vehicle), Encoding.UTF8, "application/json");
                using (var Vehicleresponse = await httpClient.PutAsync("https://localhost:7291/api/Vehicle/" + id, content1))
                {
                    string apiResponse = await Vehicleresponse.Content.ReadAsStringAsync();
                }
            }
            return Vehicle;
        }


    }
}