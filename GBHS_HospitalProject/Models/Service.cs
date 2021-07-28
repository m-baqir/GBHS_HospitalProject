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
        public string ServicePhone { get; set; }
        public string ServiceEmail { get; set; }
        public string ServiceLocation { get; set; }
        public string ServiceInfo { get; set; }
        public bool ServiceHasPic { get; set; }
        public string PicExtension { get; set; }

        //a service can have many locations
        public ICollection<Location> Locations { get; set; }
    }

    public class ServiceDto
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServicePhone { get; set; }
        public string ServiceEmail { get; set; }
        public string ServiceLocation { get; set; }
        public string ServiceInfo { get; set; }
        public bool ServiceHasPic { get; set; }
        public string PicExtension { get; set; }

        //a service can have many locations
        public ICollection<Location> Locations { get; set; }

    }
}