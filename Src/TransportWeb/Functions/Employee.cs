using Newtonsoft.Json;
using System.Text;
using TransportWeb.Models;

namespace TransportWeb.Functions
{
    public class Employee
    {
        public async Task<List<EmployeeInfo>> GetEmployee()
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7291/api/Employee");
                if (Res.IsSuccessStatusCode)
                {
                    var empress = await Res.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<EmployeeInfo>>(empress);//to convert json to list
                }
                return employees;
            }
        }
        public async Task<EmployeeInfo> GetEmployee(int Id)
        {
            EmployeeInfo emp = new EmployeeInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7291/api/Employee/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                }
                return emp;
            }
        }
        public async Task<string> AddEmployee(EmployeeInfo e)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7291/api/Employee", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    e = JsonConvert.DeserializeObject<EmployeeInfo>(apiresponse);
                    if (response.IsSuccessStatusCode)
                    {
                        await SetSeatCount(e.VehicleId, "DEC");
                        return "Employee details Successfully added..";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> UpdateEmployee(EmployeeInfo e, int OldVehicleNum)
        {

            using (var httpClient = new HttpClient())
            {

                int id = e.EmployeeId;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7291/api/Employee/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    e = JsonConvert.DeserializeObject<EmployeeInfo>(apiResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        if (e.VehicleId != OldVehicleNum)
                        {
                            await SetSeatCount(e.VehicleId, "DEC");
                            await SetSeatCount(OldVehicleNum, "INC");

                        }
                        return "Employee details updated successfully";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        public async Task<string> DeleteEmployee(EmployeeInfo EmpInfo)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7291/api/Employee/" + EmpInfo.EmployeeId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        await SetSeatCount(EmpInfo.VehicleId, "INC");
                        return "Employee details deleted successfully";
                    }
                    else
                    {
                        return $"Unable to process - {response.StatusCode}";
                    }
                }
            }
        }
        private async Task<VehicleInfo> SetSeatCount(int VehicleId, string type)
        {
            var Vehicle = new VehicleInfo();
            using (var httpClient = new HttpClient())
            {
                using (var Vehicleresponse = await httpClient.GetAsync("https://localhost:7291/api/Vehicle/" + VehicleId))
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
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(Vehicle), Encoding.UTF8, "application/json");
                using (var Vehicleresponse = await httpClient.PutAsync("https://localhost:7291/api/Vehicle/" + VehicleId, content1))
                {
                    string apiResponse = await Vehicleresponse.Content.ReadAsStringAsync();
                }
            }
            return Vehicle;
        }
    }
}
