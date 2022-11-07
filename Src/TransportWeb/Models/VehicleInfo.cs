using Microsoft.AspNetCore.Mvc.Rendering;
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
        public int VehicleId { get; set; }
        [Display(Name = "Vehicle Number")]
        [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{4}$", ErrorMessage = "Enter Valid Vehicle Number Eg:TN29AU1234")]
        public string VehicleNum { get; set; }
        public int RouteNum { get; set; }
        [Display(Name = "Seat Capacity")]
        public int Capacity { get; set; }
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }
        [Display(Name = "Status")]
        public bool IsOperable { get; set; }
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<RouteInfo> RouteInfos { get; set; }

    }
    public partial class Vehicles
    {
        public List<VehicleInfo>? VehicleList { get; set; }
        public List<RouteInfo>? RouteList { get; set; }
    }
}
