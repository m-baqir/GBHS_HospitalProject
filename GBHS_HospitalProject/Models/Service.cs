using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBHS_HospitalProject.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public float ServicePhone { get; set; }
        public string ServiceEmail { get; set; }
        public string ServiceLocation { get; set; }
        public string ServiceInfo { get; set; }
    }
}