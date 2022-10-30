using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string VehicleNum { get; set; } = null!;
        public int? StopId { get; set; }
        public int RouteNum { get; set; }

        public virtual StopInfo? Stop { get; set; }
        public virtual VehicleInfo? VehicleNumNavigation { get; set; } = null!;
    }
}
