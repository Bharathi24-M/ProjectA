using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class VehicleInfo
    {
        public VehicleInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
            RouteInfos = new HashSet<RouteInfo>();
        }

        public string VehicleNum { get; set; } = null!;
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsOperable { get; set; }

        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<RouteInfo> RouteInfos { get; set; }
    }
}
