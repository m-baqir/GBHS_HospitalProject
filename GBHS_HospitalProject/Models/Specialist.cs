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
    
    public string SpecialistFirstName { get; set; }
    public string SpecialistLastName { get; set; }
        [ForeignKey("Departments")]
    public int DepartmentID { get; set; }
    public virtual Department Departments { get; set; }
    public ICollection<Booking> SpecialistBookings { get; set; }
  }
  public class SpecialistDto
  {
    [Display(Name = "ID")]
    public int SpecialistID { get; set; }
    [Display(Name = "First Name")]
    public string SpecialistFirstName { get; set; }
    [Display(Name = "Last Name")]
    public string SpecialistLastName { get; set; }
    public int DepartmentID { get; set; }
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; }
    public ICollection<Booking> SpecialistBookings { get; set; }
  }
}