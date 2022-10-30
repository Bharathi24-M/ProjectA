using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class RouteInfo
    {
        public RouteInfo()
        {
            StopInfos = new HashSet<StopInfo>();
        }

        public int RouteNum { get; set; }
        public string VehicleNum { get; set; } = null!;
        public string? RouteName { get; set; }

        public virtual VehicleInfo ?VehicleNumNavigation { get; set; } = null!;
        public virtual ICollection<StopInfo> StopInfos { get; set; }
    }
}
