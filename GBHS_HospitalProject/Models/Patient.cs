using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string PatientGender { get; set; }

        ICollection<Booking> PatientBookings { get; set; }

    }

    //DTO
    public class PatientDto
    {
        public int PatientID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string PatientGender { get; set; }
    }
}