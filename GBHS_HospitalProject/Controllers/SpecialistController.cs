using GBHS_HospitalProject.Models;
using GBHS_HospitalProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GBHS_HospitalProject.Controllers
{
  public class SpecialistController : Controller
  {
    private static readonly HttpClient client;
    private JavaScriptSerializer jss = new JavaScriptSerializer();
    static SpecialistController()
    {
      client = new HttpClient();
      client.BaseAddress = new Uri("https://localhost:44389/api/");
    }
    // GET: Specialist/List
    public ActionResult List()
    {
      string url = "specialistsdata/listspecialists";

      HttpResponseMessage response = client.GetAsync(url).Result;
      IEnumerable<SpecialistDto> specialists = response.Content.ReadAsAsync<IEnumerable<SpecialistDto>>().Result;

      return View(specialists);
    }

    // GET: Specialist/Details/5
    public ActionResult Details(int id)
    {
      string url = "specialistsdata/findspecialist/" + id;

      HttpResponseMessage response = client.GetAsync(url).Result;
      SpecialistDto SelectedSpecialist = response.Content.ReadAsAsync<SpecialistDto>().Result;

      if (SelectedSpecialist == null)
      {
        return HttpNotFound();
      }

      return View(SelectedSpecialist);
    }

    public ActionResult Error()
    {

      return View();
    }

    // GET: Specialist/Create
    public ActionResult Create()
    {
      SpecialistDetails ViewModel = new SpecialistDetails();

      string url = "departmentsdata/listdepartments";
      HttpResponseMessage response = client.GetAsync(url).Result;
      IEnumerable<Department> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;
      ViewModel.RelatedDepartments = DepartmentOptions;

      return View(ViewModel);
    }

    // POST: Specialist/Create
    [HttpPost]
    public ActionResult Create(Specialist specialist)
    {
      string url = "Specialistsdata/addspecialist/";

      string jsonpayload = jss.Serialize(specialist);
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

    // GET: Specialist/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Specialist/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Specialist/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Specialist/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
