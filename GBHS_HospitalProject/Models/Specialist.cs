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
    //[Required]
    public string SpecialistFirstName { get; set; }
    //[Required]
    public string SpecialistLastName { get; set; }
        [ForeignKey("Departments")]
    //[Required]
    public int DepartmentID { get; set; }
    public virtual Department Departments { get; set; }
    public ICollection<Booking> SpecialistBookings { get; set; }
  }
  public class SpecialistDto
  {
    [Display(Name = "ID")]
    public int SpecialistID { get; set; }
    [Display(Name = "First Name")]
    //[Required(ErrorMessage ="Please enter First Name")]
    public string SpecialistFirstName { get; set; }
    [Display(Name = "Last Name")]
    //[Required(ErrorMessage = "Please enter Last Name")]
    public string SpecialistLastName { get; set; }
    //[Required(ErrorMessage = "Please choose a Department")]
    public int DepartmentID { get; set; }
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; }
    public ICollection<Booking> SpecialistBookings { get; set; }
  }
}