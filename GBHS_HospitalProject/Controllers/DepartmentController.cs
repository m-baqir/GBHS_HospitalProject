using GBHS_HospitalProject.Models;
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
    public ActionResult List()
    {
      string url = "departmentsdata/listdepartments";

      HttpResponseMessage response = client.GetAsync(url).Result;
      IEnumerable<Department> departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

      return View(departments);
    }

    // GET: Department/Details/5
    public ActionResult Details(int id)
    {
      string url = "departmentsdata/finddepartment/" + id;

      HttpResponseMessage response = client.GetAsync(url).Result;
      Department SelectedDepartment = response.Content.ReadAsAsync<Department>().Result;
      //Debug.WriteLine(SelectedDepartment.DepartmentName);
      if (SelectedDepartment == null)
      {
        return HttpNotFound();
      }

      return View(SelectedDepartment);
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
      return View();
    }

    // POST: Department/Edit/5
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

    // GET: Department/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Department/Delete/5
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
