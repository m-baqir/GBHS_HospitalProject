using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GBHS_HospitalProject.Models;

namespace GBHS_HospitalProject.Controllers
{
  public class DepartmentsDataController : ApiController
  {
    private HospitalDbContext db = new HospitalDbContext();

   
    /// <summary>
    /// List all the department available in the system
    /// </summary>
    /// <returns>
    /// HEADER:200
    /// CONTENT: All the department currently in the database
    /// </returns>
    /// <example>
    /// GET: api/DepartmentsData/ListDepartments
    /// </example>
    [HttpGet]
    [ResponseType(typeof(Department))]

    public IEnumerable<Department> ListDepartments()
    {
      return db.Departments;
    }

    /// <summary>
    /// Find a department with its ID
    /// </summary>
    /// <param name="id">The id of the department</param>
    /// <returns>
    /// HEADER: 200
    /// CONTENT: The department that has its ID match the specified ID 
    /// or
    /// HEADER: 404
    /// CONTENT NOT FOUND
    /// </returns>
    /// <example>
    /// GET: api/DepartmentsData/FindDepartments/1
    /// </example>
    [HttpGet]
    [ResponseType(typeof(Department))]
    public IHttpActionResult FindDepartment(int id)
    {
      Department department = db.Departments.Find(id);
      if (department == null)
      {
        return NotFound();
      }

      return Ok(department);
    }

    /// <summary>
    /// Update the information of a department that match the specified ID
    /// </summary>
    /// <param name="id">The ID of the department</param>
    /// <param name="department">JSON FORM DATA of the department</param>
    /// <returns>
    /// HEADER: 204 (Success, no content response)
    /// or
    /// HEADER: 400 (Bad request)
    /// or
    /// HEADER: 404 (Not Found)
    /// </returns>
    /// <example>
    /// POST api/DepartmentsData/Update/5
    /// </example>
    [ResponseType(typeof(void))]
    [HttpPost]
    public IHttpActionResult UpdateDepartment(int id, Department department)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != department.DepartmentID)
      {
        return BadRequest();
      }

      db.Entry(department).State = EntityState.Modified;

      try
      {
        db.SaveChanges();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!DepartmentExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Add a new department into the database
    /// </summary>
    /// <param name="department">JSON FORM DATA of the department</param>
    /// <returns>
    /// HEADER: 201 (Created)
    /// or
    /// HEADER: 400 (Bad request)
    /// </returns>
    /// <example>
    /// POST: api/DepartmentsData/AddDepartment
    /// </example>
    [ResponseType(typeof(Department))]
    [HttpPost]
    public IHttpActionResult AddDepartment(Department department)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.Departments.Add(department);
      db.SaveChanges();

      return CreatedAtRoute("DefaultApi", new { id = department.DepartmentID }, department);
    }

    // 
    /// <summary>
    /// Delete a Department that match the specified ID
    /// </summary>
    /// <param name="id">The ID of the Department that intended to be deleted</param>
    /// <returns>
    /// HEADER: 200 (OK)
    /// or
    /// HEADER: 404 (BAD REQUEST)
    /// </returns>
    /// <example>
    /// DELETE: api/DepartmentsData/5
    /// </example>
    [ResponseType(typeof(Department))]
    [HttpPost]
    public IHttpActionResult DeleteDepartment(int id)
    {
      Department department = db.Departments.Find(id);
      if (department == null)
      {
        return NotFound();
      }

      db.Departments.Remove(department);
      db.SaveChanges();

      return Ok(department);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool DepartmentExists(int id)
    {
      return db.Departments.Count(e => e.DepartmentID == id) > 0;
    }
  }
}