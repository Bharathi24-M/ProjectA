using System;
using System.Collections.Generic;

namespace TransportApi.Models
{
    public partial class StopInfo
    {
        public StopInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
        }

        public int StopId { get; set; }
        public int RouteNum { get; set; }
        public string StopName { get; set; } = null!;

        public virtual RouteInfo? RouteNumNavigation { get; set; } = null!;
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
    }
}
