using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
  public class SpecialistDetails
  {
    public SpecialistDto SelectedSpecialist{ get; set; }
    public IEnumerable<Department> RelatedDepartments { get; set; }
  }
}