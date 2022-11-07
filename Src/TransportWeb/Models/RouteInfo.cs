using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TransportWeb.Models
{
    public partial class RouteInfo
    {
        [Display(Name = "Route Number")]
        public int RouteNum { get; set; }
        [Display(Name = "Route Name")]
        [RegularExpression(@"^[A-Z]{1}[a-z]+[/-][A-z]{1,2}[a-z]+$", ErrorMessage = "Enter the valid Route Name: Eg:Guindy-Porur")]
        public string RouteName { get; set; }       
        public List<SelectListItem>? RouteList { get; set; }
        public List<SelectListItem>? VehicleList { get; set; }

    }

}
