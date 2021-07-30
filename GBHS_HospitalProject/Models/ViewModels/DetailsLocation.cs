using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
    public class DetailsLocation
    {
        public LocationDto SelectedLocation { get; set; }
        public IEnumerable<ServiceDto> ServicesAtLocation { get; set; }
        public IEnumerable<ServiceDto> ServicesNotAtLocation { get; set; }
    }
}