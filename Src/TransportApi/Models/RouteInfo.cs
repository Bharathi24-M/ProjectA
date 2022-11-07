using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class RouteInfo
    {
        public RouteInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
            StopInfos = new HashSet<StopInfo>();
            VehicleInfos = new HashSet<VehicleInfo>();
        }

        public int RouteNum { get; set; }
        public string RouteName { get; set; } = null!;

        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<StopInfo> StopInfos { get; set; }
        public virtual ICollection<VehicleInfo> VehicleInfos { get; set; }
    }
}
