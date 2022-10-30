using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class VehicleController : Controller
    {
        public static List<VehicleInfo> VehicleInfo = new List<VehicleInfo>();
        public async Task<IActionResult> Index()
        {
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
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Vehicle");
                if (Res.IsSuccessStatusCode)
                {
                    var empress = Res.Content.ReadAsStringAsync().Result;
                    VehicleInfo = JsonConvert.DeserializeObject<List<VehicleInfo>>(empress);//to convert json to list
                }
            }
            return View(VehicleInfo);
        }
        public IActionResult AddVehicleDetails()
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleDetails(VehicleInfo e)
        {
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
                    using (var response = await client.PostAsync("https://localhost:7291/api/Vehicle", content))
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        var Vehicleobj = JsonConvert.DeserializeObject<VehicleInfo>(apiresponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.AddVehiclestatus = "Vehicle details added Successfully..";
                        }
                        else
                        {
                            ViewBag.AddVehiclestatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AddVehiclestatus = "Something Went Wrong";
                return View();
            }

            return View(e);
            //return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetVehicleDetails(string id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            VehicleInfo Vehicle = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Vehicle = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                }

            }
            return View(Vehicle);
        }

        public async Task<IActionResult> EditVehicleDetails(string id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            VehicleInfo Vehicle = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Vehicle = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                }
            }
            return View(Vehicle);
        }
        [HttpPost]
        public async Task<ActionResult> EditVehicleDetails(VehicleInfo e)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                VehicleInfo receiveVehicle = new VehicleInfo();

                using (var httpClient = new HttpClient())
                {

                    string id = e.VehicleNum;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7291/api/Vehicle/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receiveVehicle = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.EditVehicleStatus = "Vehicle details updated successfully";
                        }
                        else
                        {
                            ViewBag.EditVehicleStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.EditVehicleStatus = "Something went wrong";
            }
            return View(e);
            //return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> DeleteVehicleDetails(string id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }

            VehicleInfo e = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TempData["VehicleNum"] = apiResponse;
                    e = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                }
            }

            return View(e);
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
                Vehicle = JsonConvert.DeserializeObject<VehicleInfo>(TempData["VehicleNum"].ToString());

                if (Vehicle == null)
                {
                    Vehicle = new VehicleInfo();
                }
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Vehicle/" + Vehicle.VehicleNum))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ViewBag.DeleteVehicleStatus = "Vehicle details deleted successfully";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                        {
                            ViewBag.DeleteVehicleStatus = "Vehicle number is used, Please delete it from route then try again..!";
                        }
                        else
                        {
                            ViewBag.DeleteRouteStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }

                }
            }
            catch(Exception)
             {
                ViewBag.DeleteVehicleStatus = "Something went wrong";
            }
            
            return View(Vehicle);
        }
    }
}
