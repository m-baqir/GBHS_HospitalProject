using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
    public class DetailsPatient
    {
        public PatientDto SelectedPatient { get; set; }
        public IEnumerable<BookingDto> BookingsOfPatient;
    }
}