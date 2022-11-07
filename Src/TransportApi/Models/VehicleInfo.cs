using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class VehicleInfo
    {
        public VehicleInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
        }

        public int VehicleId { get; set; }
        public string VehicleNum { get; set; } = null!;
        public int RouteNum { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsOperable { get; set; }

        public virtual RouteInfo? RouteNumNavigation { get; set; } 
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
    }
}
