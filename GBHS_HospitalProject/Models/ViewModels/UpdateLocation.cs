using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
    public class UpdateLocation
    {
        public LocationDto SelectedLocation { get; set; }
        public IEnumerable<ServiceDto> ServicesOptions { get; set; }
    }
}