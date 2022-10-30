using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class RouteController : Controller
    {
        public static List<RouteInfo> RouteInfo = new List<RouteInfo>();
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
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteResult = Res.Content.ReadAsStringAsync().Result;
                    RouteInfo = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteResult);//to convert json to list
                }
            }
            return View(RouteInfo);
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
            using (var client = new HttpClient())
            {
                List<SelectListItem> Routelist = new List<SelectListItem>();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage RouteResult = await client.GetAsync("https://localhost:7291/api/vehicle");
                if (RouteResult.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = RouteResult.Content.ReadAsStringAsync().Result;
                    RouteInfo.VehicleList = (JsonConvert.DeserializeObject<List<VehicleInfo>>(RouteJsonResponse)).Select(x => new SelectListItem()
                    {
                        Text = x.VehicleNum,
                        Value = x.VehicleNum
                    }).ToList();
                }
            }
            return View(RouteInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddRouteDetails(RouteInfo e)
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
                    using (var response = await client.PostAsync("https://localhost:7291/api/Route", content))
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        var Routeobj = JsonConvert.DeserializeObject<RouteInfo>(apiresponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.AddRouteStatus = "Route details successfully added";
                        }
                        else
                        {
                            ViewBag.AddRouteStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AddRouteStatus = "Something went Wrong";
            }

            return View(e);
            // return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetRouteDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            RouteInfo Route = new RouteInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Route/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Route = JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                }

            }
            return View(Route);
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
            List<SelectListItem> vehiclelist = new List<SelectListItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Route/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Route = JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                }
            }
            using (var client = new HttpClient())
            {
                List<SelectListItem> Vehiclelist = new List<SelectListItem>();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage VehicleResult = await client.GetAsync("https://localhost:7291/api/Vehicle");
                if (VehicleResult.IsSuccessStatusCode)
                {
                    var VehicleJsonResponse = VehicleResult.Content.ReadAsStringAsync().Result;
                    Route.VehicleList = (JsonConvert.DeserializeObject<List<VehicleInfo>>(VehicleJsonResponse)).Select(x => new SelectListItem()
                    {
                        Text = x.VehicleNum,
                        Value = x.VehicleNum
                    }).ToList();
                }
            }
           
            return View(Route);
        }
        [HttpPost]
        public async Task<ActionResult> EditRouteDetails(RouteInfo e)
        {
            try
            {
                if (HttpContext.Session.GetString("UserName") == null)
                {
                    var error = new ErrorViewModel();
                    error.Errorcode = 401;
                    return View("Error", error);
                }
                RouteInfo receivedRoute = new RouteInfo();

                using (var httpClient = new HttpClient())
                {

                    int id = e.RouteNum;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7291/api/Route/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        receivedRoute = JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.EditRouteStatus = "Updated the Route details successfully..";
                        }
                        else
                        {
                            ViewBag.EditRouteStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.EditRouteStatus = "Something went Wrong";
            }

            return View(e);

            // return RedirectToAction("Index");
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
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Route/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TempData["RouteNum"] = apiResponse;
                    Route = JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                }
            }
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
                Route = JsonConvert.DeserializeObject<RouteInfo>(TempData["RouteNum"].ToString());
                if (Route == null)
                {
                    Route = new RouteInfo();
                }
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Route/" + Route.RouteNum))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.DeleteRouteStatus = "Route details deleted successfully";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                        {
                            ViewBag.DeleteRouteStatus = "This Route is used, Please delete it from Stop then try again..!";
                        }
                        else
                        {
                            ViewBag.DeleteRouteStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch
            {
                ViewBag.DeleteRouteStatus = "Something went wrong";
            }

            return View(Route);
        }

    }
}
