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
  public class SpecialistsDataController : ApiController
  {
    private ApplicationDbContext db = new ApplicationDbContext();


    /// <summary>
    /// List all specialist and their corresponding deparment
    /// </summary>
    /// <returns>
    /// HEADER:200
    /// CONTENT: All the specialist and their information currently in the database
    /// </returns>
    /// <example>
    /// // GET: api/SpecialistsData/ListSpecialists
    /// </example>
    /// [HttpGet]
    [ResponseType(typeof(Specialist))]
    public IEnumerable<SpecialistDto> ListSpecialists()
    {
      List<Specialist> Specialists = db.Specialists.ToList();
      List<SpecialistDto> SpecialistDtos = new List<SpecialistDto>();

      Specialists.ForEach(s => SpecialistDtos.Add(new SpecialistDto()
      {
        SpecialistID = s.SpecialistID,
        SpecialistFirstName = s.SpecialistFirstName,
        SpecialistLastName = s.SpecialistLastName,
        DepartmentID = s.Departments.DepartmentID,
        DepartmentName = s.Departments.DepartmentName
      }));
      return SpecialistDtos;
    }


    /// <summary>
    /// Find the Specialist with the corresponding ID
    /// </summary>
    /// <param name="id">Specialist ID</param>
    /// <returns>
    /// HEADER: 200
    /// CONTENT: The specialist that has their ID matched the specified ID 
    /// or
    /// HEADER: 404
    /// CONTENT NOT FOUND
    /// </returns>
    /// <example>
    /// // GET: api/SpecialistsData/FindSpecialist/1
    /// </example>
    [HttpGet]
    [ResponseType(typeof(Specialist))]
    public IHttpActionResult FindSpecialist(int id)
    {
      Specialist Specialist = db.Specialists.Find(id);
      SpecialistDto SpecialistDto = new SpecialistDto()
      {
        SpecialistID = Specialist.SpecialistID,
        SpecialistFirstName = Specialist.SpecialistFirstName,
        SpecialistLastName = Specialist.SpecialistLastName,
        DepartmentID = Specialist.Departments.DepartmentID,
        DepartmentName = Specialist.Departments.DepartmentName
      };
      if (Specialist == null)
      {
        return NotFound();
      }

      return Ok(SpecialistDto);
    }

    /// <summary>
    /// Update the information of a specialist that match the specified ID
    /// </summary>
    /// <param name="id">The ID of the specialist</param>
    /// <param name="specialist">JSON FORM DATA of the specialist</param>
    /// <returns>
    /// HEADER: 204 (Success, no content response)
    /// or
    /// HEADER: 400 (Bad request)
    /// or
    /// HEADER: 404 (Not Found)
    /// </returns>
    /// <example>
    /// POST api/SpecialistsData/Update/5
    /// </example>
    [ResponseType(typeof(void))]
    public IHttpActionResult UpdateSpecialist(int id, Specialist specialist)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != specialist.SpecialistID)
      {
        return BadRequest();
      }

      db.Entry(specialist).State = EntityState.Modified;

      try
      {
        db.SaveChanges();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!SpecialistExists(id))
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
    /// Add a new Specialist into the database
    /// </summary>
    /// <param name="specialist">JSON FORM DATA of the specialist</param>
    /// <returns>
    /// HEADER: 201 (Created)
    /// or
    /// HEADER: 400 (Bad request)
    /// </returns>
    /// <example>
    /// POST: api/SpecialistsData/AddSpecialist
    /// </example>
    [ResponseType(typeof(Specialist))]
    [HttpPost]
    public IHttpActionResult AddSpecialist(Specialist specialist)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.Specialists.Add(specialist);
      db.SaveChanges();

      return CreatedAtRoute("DefaultApi", new { id = specialist.SpecialistID }, specialist);
    }

    /// <summary>
    /// Delete a Specialist that match the specified ID
    /// </summary>
    /// <param name="id">The ID of the Specialist that intended to be deleted</param>
    /// <returns>
    /// HEADER: 200 (OK)
    /// or
    /// HEADER: 404 (BAD REQUEST)
    /// </returns>
    /// <example>
    /// DELETE: api/SpecialistsData/5
    /// </example>
    [ResponseType(typeof(Specialist))]
    [HttpPost]
    public IHttpActionResult DeleteSpecialist(int id)
    {
      Specialist specialist = db.Specialists.Find(id);
      if (specialist == null)
      {
        return NotFound();
      }

      db.Specialists.Remove(specialist);
      db.SaveChanges();

      return Ok(specialist);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool SpecialistExists(int id)
    {
      return db.Specialists.Count(e => e.SpecialistID == id) > 0;
    }
  }
}