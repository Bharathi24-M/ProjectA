using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int RouteNum { get; set; }
        public int VehicleId { get; set; }
        public int StopId { get; set; }

        public virtual RouteInfo? RouteNumNavigation { get; set; } 
        public virtual StopInfo? Stop { get; set; } 
        public virtual VehicleInfo? Vehicle { get; set; } 
    }
}
