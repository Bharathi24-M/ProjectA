using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Functions
{
    public class Routes
    {
        public async Task<List<RouteInfo>> GetRoute()
        {
            var routes = new List<RouteInfo>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Route");
                if (Res.IsSuccessStatusCode)
                {
                    var RouteJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    routes = JsonConvert.DeserializeObject<List<RouteInfo>>(RouteJsonResponse);
                }
                return routes;
            }
        }
        public async Task<RouteInfo> GetRoute(int id)
        {
            RouteInfo Route = new RouteInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Route/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                }

            }
        }
        public async Task<string> AddRoute(RouteInfo Route)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Route), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7291/api/Route", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    var Routeobj = JsonConvert.DeserializeObject<RouteInfo>(apiresponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Route details successfully added";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return $"{Route.RouteName} route already exits";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> UpdateRoute(RouteInfo Route)
        {
            using (var httpClient = new HttpClient())
            {
                int id = Route.RouteNum;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(Route), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7291/api/Route/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Route = JsonConvert.DeserializeObject<RouteInfo>(apiResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Updated the Route details successfully..";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> DeleteRoute(RouteInfo Route)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Route/" + Route.RouteNum))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return "Route details deleted successfully";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                    {
                        return "This Route is used, Please delete it from Stop then try again..!";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
    }
}
