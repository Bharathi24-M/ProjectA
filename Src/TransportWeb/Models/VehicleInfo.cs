using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TransportWeb.Models
{
    public partial class VehicleInfo
    {
        public VehicleInfo()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
            RouteInfos = new HashSet<RouteInfo>();
        }
        [Display(Name = "Vehicle Number")]
        public string VehicleNum { get; set; } = null!;
        [Display(Name = "Seat Capacity")]
        public int Capacity { get; set; }
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }
        [Display(Name = "Status")]
        public bool IsOperable { get; set; }

        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<RouteInfo> RouteInfos { get; set; }
    }
}
