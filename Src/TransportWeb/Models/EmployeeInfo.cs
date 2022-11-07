using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransportWeb.Models
{
    public partial class EmployeeInfo
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]+[a-zA-Z]$", ErrorMessage = "Name should be in alphabet")]
        public string Name { get; set; } = null!;
        [Range(18, 70), Required]
        public int? Age { get; set; }
        [Required, RegularExpression(@"^[6-9]{1}[0-9]{9}$", ErrorMessage = "Enter Valid Mobile Number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Vehicle Number"), Required]
        public int VehicleId { get; set; }
        public int StopId { get; set; }
        [Display(Name = "Route Name"), Required]
        public int RouteNum { get; set; }
        public List<RouteInfo>? RouteList { get; set; }
        public List<StopInfo>? StopList { get; set; }
        public List<VehicleInfo>? VehicleList { get; set; }


    }
    public partial class EmployeeList
    {
        public List<EmployeeInfo>? EmployeeInfoList { get; set; }
        public List<SelectListItem>? RouteList { get; set; }
        public List<StopInfo>? StopList { get; set; }
        public List<VehicleInfo>? VehicleList { get; set; }
    }

}
