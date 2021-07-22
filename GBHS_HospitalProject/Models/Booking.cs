using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models
{
    public class Booking
    {
        [Key]
        public string BookingID { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public string BookingReasonToVist { get; set; }

        [ForeignKey("Patient")]
        public int? PatientID;
        public virtual Patient Patient { get; set; }

        [ForeignKey("Specialist")]
        public int? SpecialistID;
        public virtual Specialist Specialist { get; set; }
    }
}



