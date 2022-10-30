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
        public string RouteName { get; set; }
        [Display(Name = "Vehicle Number")]
        public string VehicleNum { get; set; }
        public List<SelectListItem>? RouteList { get; set; }
        public List<SelectListItem>? VehicleList { get; set; }

    }

}
