using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Functions
{
    public class Vehicle
    {
        public async Task<List<VehicleInfo>> GetVehicle()
        {
            var vehicles = new List<VehicleInfo>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Vehicle");
                if (Res.IsSuccessStatusCode)
                {
                    var VehicleJsonResponse = Res.Content.ReadAsStringAsync().Result;
                    vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(VehicleJsonResponse);
                }
                return vehicles;
            }
        }
        public async Task<VehicleInfo> GetVehicle(int id)
        {
            VehicleInfo Vehicle = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                   return JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                }
            }
        }
        public async Task<string> AddVehicle(VehicleInfo Vehicle)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Vehicle), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7291/api/Vehicle", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    var Vehicleobj = JsonConvert.DeserializeObject<VehicleInfo>(apiresponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Vehicle details added Successfully..";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return $"Vehicle Number {Vehicle.VehicleNum} already exists in the selected route";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> UpdateVehicle(VehicleInfo Vehicle)
        {           
            using (var httpClient = new HttpClient())
            {
                int id = Vehicle.VehicleId;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(Vehicle), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7291/api/Vehicle/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Vehicle = JsonConvert.DeserializeObject<VehicleInfo>(apiResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return "Vehicle details updated successfully";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        return $"Vehicle Number {Vehicle.VehicleNum} already exists in the selected route";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> DeleteVehicle(VehicleInfo Vehicle)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Vehicle/" + Vehicle.VehicleId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                       return "Vehicle details deleted successfully";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.FailedDependency)
                    {
                        return "Vehicle number is used, Please delete it from route then try again..!";
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
