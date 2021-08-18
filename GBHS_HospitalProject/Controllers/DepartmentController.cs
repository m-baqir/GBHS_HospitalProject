using GBHS_HospitalProject.Models;
using GBHS_HospitalProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GBHS_HospitalProject.Controllers
{
  public class DepartmentController : Controller
  {
    private static readonly HttpClient client;
    private JavaScriptSerializer jss = new JavaScriptSerializer();
    static DepartmentController()
    {
      client = new HttpClient();
      client.BaseAddress = new Uri("https://localhost:44389/api/");
    }
    // GET: Department/List
    [Authorize(Roles = "Admin,Guest")]
    public ActionResult List()
    {
      string url = "departmentsdata/listdepartments";

      HttpResponseMessage response = client.GetAsync(url).Result;
      IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

      return View(departments);
    }

    // GET: Department/Details/5
    [Authorize(Roles = "Admin,Guest")]
    public ActionResult Details(int id)
    {
      DetailsDepartment ViewModel = new DetailsDepartment();

      string url = "departmentsdata/finddepartment/" + id;

      HttpResponseMessage response = client.GetAsync(url).Result;
      DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
      ViewModel.selectedDepartment = SelectedDepartment;
      //Debug.WriteLine(SelectedDepartment.DepartmentName);

      url = "specialistsdata/listspecialistsfordepartment/" + id;
      response = client.GetAsync(url).Result;
      IEnumerable<SpecialistDto> RelatedSpecialists = response.Content.ReadAsAsync<IEnumerable<SpecialistDto>>().Result;
      ViewModel.relatedSpecialists = RelatedSpecialists;

      return View(ViewModel);
    }

    public ActionResult Error()
    {

      return View();
    }

    // GET: Department/Create
    public ActionResult Create()
    {

      return View();
    }

    // POST: Department/Create
    [HttpPost]
    public ActionResult Create(Department department)
    {
      string url = "departmentsdata/adddepartment/";

      string jsonpayload = jss.Serialize(department);
      HttpContent content = new StringContent(jsonpayload);
      content.Headers.ContentType.MediaType = "application/json";

      HttpResponseMessage response = client.PostAsync(url, content).Result;
      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("List");
      }
      else
      {
        return RedirectToAction("Error");
      }
    }

    // GET: Department/Edit/5
    public ActionResult Edit(int id)
    {
      string url = "departmentsdata/finddepartment/" + id;
      HttpResponseMessage response = client.GetAsync(url).Result;
      Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;



      return View(SelectedDepartment);
    }

    // POST: Department/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, Department department)
    {
      string url = "departmentsdata/updatedepartment/" + id;
      string jsonpayload = jss.Serialize(department);

      HttpContent content = new StringContent(jsonpayload);

      content.Headers.ContentType.MediaType = "application/json";
      HttpResponseMessage response = client.PostAsync(url, content).Result;

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("List");
      }
      else
      {
        return RedirectToAction("Error");
      }
    }

    // GET: Department/Delete/5
    public ActionResult Delete(int id)
    {
      string url = "departmentsdata/finddepartment/" + id;
      HttpResponseMessage response = client.GetAsync(url).Result;
      Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
      return View(SelectedDepartment);
    }

    // POST: Department/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, Department department)
    {
      string url = "departmentsdata/deletedepartment/" + id;
      string jsonpayload = jss.Serialize(department);
      HttpContent content = new StringContent(jsonpayload);
      content.Headers.ContentType.MediaType = "application/json";
      HttpResponseMessage response = client.PostAsync(url, content).Result;

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("List");
      }
      else
      {
        return RedirectToAction("Error");
      }
    }
  }
}
