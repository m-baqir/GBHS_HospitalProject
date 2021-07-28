using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
    public class DetailsService
    {
        public ServiceDto SelectedService { get; set; }
        public IEnumerable<LocationDto> LocationsWithService { get; set; }
        public IEnumerable<LocationDto> LocationsWithoutService { get; set; }
    }
}