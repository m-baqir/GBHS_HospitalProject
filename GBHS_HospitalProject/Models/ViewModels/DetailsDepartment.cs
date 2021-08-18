using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBHS_HospitalProject.Models.ViewModels
{
  public class DetailsDepartment
  {
    public DepartmentDto selectedDepartment { get; set; }
    public IEnumerable<SpecialistDto> relatedSpecialists { get; set; }
  }
}