using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Functions
{
    public class Stop
    {
        public async Task<List<StopInfo>> GetStop()
        {
            var stopList = new List<StopInfo>();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Stop");
                if (Res.IsSuccessStatusCode)
                {
                    var stopjson = Res.Content.ReadAsStringAsync().Result;
                    stopList = JsonConvert.DeserializeObject<List<StopInfo>>(stopjson).ToList();
                }
                return stopList;
            }
        }
        public async Task<StopInfo> GetStop(int id)
        {
            StopInfo Stop = new StopInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Stop/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<StopInfo>(apiResponse);
                }

            }
        }
        public async Task<string> AddStop(StopInfo Stop)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Stop), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7291/api/Stop", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    var Stopobj = JsonConvert.DeserializeObject<StopInfo>(apiresponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Stop details successfully added";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return $"Stop Name {Stop.StopName} already exists";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<String> UpdateStop(StopInfo Stop)
        {
            using (var httpClient = new HttpClient())
            {
                int id = Stop.StopId;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(Stop), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7291/api/Stop/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Stop = JsonConvert.DeserializeObject<StopInfo>(apiResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Stop details updated successfully";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> DeleteStop(StopInfo Stop)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Stop/" + Stop.StopId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {

                        return "Stop details deleted successfully";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                    {
                        return "This Stop is assigned to some Employee, Please remove and try again..!";
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
