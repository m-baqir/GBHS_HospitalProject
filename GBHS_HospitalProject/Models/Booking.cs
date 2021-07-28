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
        public int BookingID { get; set; }
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

    //DTO class
    public class BookingDto
    {
        public int BookingID { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public string BookingReasonToVisit { get; set; }
        public int PatientID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int SpecialistID { get; set; }
        public string SpecialistFirstName { get; set; }
        public string SpecialistLastName { get; set; }
    }
}



