using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBHS_HospitalProject.Models
{
    public class LocationxService
    {
        [Key]
        public int LocationxServiceID { get; set; }

        [ForeignKey("Location")]
        public int LocationID { get; set; }
        public virtual Location Location { get; set; }

        [ForeignKey("Service")]
        public int ServiceID { get; set; }
        public virtual Service Service { get; set; }
    }
}