using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models
{
  public class Specialist
  {
    [Key]
    public int SpecialistID { get; set;}
    public string SpecialistName { get; set; }
    [ForeignKey("Departments")]
    public int DepartmentID { get; set; }
    public virtual Department Departments { get; set; }
    public ICollection<Booking> SpecialistBookings { get; set; }
  }
  public class SpecialistDto
  {
    public int SpecialistID { get; set; }
    public string SpecialistName { get; set; }
    public int DepartmentID { get; set; }
    public string DepartmentName { get; set; }
  }
}