using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TransportWeb.Models
{
    public partial class StopInfo
    {
        [Display(Name = "Stop Id")]
        public int StopId { get; set; }
        [Display(Name = "Route Number")]
        public int RouteNum { get; set; }
        [Display(Name = "Stop Name")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z& ]+[a-zA-Z]$", ErrorMessage = "Enter the valid Stop Name")]
        public string StopName { get; set; } = null!;
        public List<SelectListItem>? RouteList { get; set; }   
    }
    public class StopList
    {
        public List<StopInfo>? StopInfoList { get; set; }
        public List<SelectListItem>? RouteList { get; set; }
    }
}
