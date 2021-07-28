using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
    public class UpdateService
    {
        public ServiceDto SelectedService { get; set; }

        public IEnumerable<LocationDto> LocationOptions { get; set; }
    }
}