using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBHS_HospitalProject.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public float LocationPhone { get; set; }
        public string LocationEmail { get; set; }
        public string LocationAddress { get; set; }

        //a location can have many services
        public ICollection<Service> Services { get; set; }
    }
}