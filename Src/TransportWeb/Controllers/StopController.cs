using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Controllers
{
    public class StopController : Controller
    {

        public async Task<IActionResult> Index()
        {
            var StopInfoList = new Stop();
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
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Stop");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteResult = Res.Content.ReadAsStringAsync().Result;
                    StopInfoList.StopList = JsonConvert.DeserializeObject<List<StopInfo>>(RouteResult);//to convert json to list
                }
            }

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage RouteResult = await client.GetAsync("https://localhost:7291/api/Route");
                if (RouteResult.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = RouteResult.Content.ReadAsStringAsync().Result;
                    StopInfoList.RouteList = (JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse)).Select(x => new SelectListItem()
                    {
                        Text = x.RouteName,
                        Value = x.RouteNum.ToString()
                    }).ToList();
                }
            }
            return View(StopInfoList);
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
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    var Routelist = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse);
                    StopInfo.RouteList = Routelist.Select(x => new SelectListItem()
                    {
                        Text = x.RouteName,
                        Value = x.RouteNum.ToString()
                    }).ToList();
                }
            }
            return View(StopInfo);
        }
        [HttpPost]
        public async Task<IActionResult> AddStopDetails(StopInfo e)
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
                    using (var response = await client.PostAsync("https://localhost:7291/api/Stop", content))
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        var Stopobj = JsonConvert.DeserializeObject<StopInfo>(apiresponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.AddStopStatus = "Stop details successfully added";
                        }
                        else
                        {
                            ViewBag.AddStopStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AddStopStatus = "Something went Wrong";
            }
           
            return View(e);

            //return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetStopDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            StopInfo Stop = new StopInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Stop/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Stop = JsonConvert.DeserializeObject<StopInfo>(apiResponse);
                }

            }
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
            StopInfo Stop = new StopInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Stop/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Stop = JsonConvert.DeserializeObject<StopInfo>(apiResponse);
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
                    Stop.RouteList = Routelist.Select(x => new SelectListItem()
                    {
                        Text = x.RouteName,
                        Value = x.RouteNum.ToString()
                    }).ToList();
                }
            }
            return View(Stop);
        }
        [HttpPost]
        public async Task<ActionResult> EditStopDetails(StopInfo Sobj)
        {
            StopInfo receivedStop = new StopInfo();
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

                    int id = Sobj.StopId;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(Sobj), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7291/api/Stop/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();                        
                        receivedStop = JsonConvert.DeserializeObject<StopInfo>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.EditStopStatus = "Stop details updated successfully";
                        }
                        else
                        {
                            ViewBag.EditStopStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
               
            }
            catch (Exception)
            {
                ViewBag.EditStopStatus = "Something went wrong!";
            }
            return View(Sobj);
            //return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> DeleteStopDetails(int id)
        {

            if (HttpContext.Session.GetString("UserName") == null)
            {
                var error = new ErrorViewModel();
                error.Errorcode = 401;
                return View("Error", error);
            }
            StopInfo Stop = new StopInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Stop/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TempData["StopId"] = apiResponse;
                    Stop = JsonConvert.DeserializeObject<StopInfo>(apiResponse);
                }
            }
            return View(Stop);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteStopDetails(StopInfo Route)
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
                if (StopInfo == null)
                {
                    StopInfo = new StopInfo();
                }
                StopInfo = JsonConvert.DeserializeObject<StopInfo>(TempData["StopId"].ToString());
            
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Stop/" + StopInfo.StopId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            
                            ViewBag.DeleteStopStatus = "Stop details deleted successfully";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                        {
                            ViewBag.DeleteStopStatus = "This Stop is assigned to some Employee, Please remove and try again..!";
                        }
                        else
                        {
                            ViewBag.DeleteStopStatus = $"Unable to process - {response.StatusCode}";
                        }
                    }
                }
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
