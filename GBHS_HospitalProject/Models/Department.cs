using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models
{
  public class Department
  {
    [Key]
    [Display(Name = "ID")]
    public int DepartmentID { get; set; }
    [Display(Name = "Department Name")]
  
    public string DepartmentName { get; set; }
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

  }
}