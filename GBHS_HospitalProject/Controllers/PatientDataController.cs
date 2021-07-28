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
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();
            Patients.ForEach(p => PatientDtos.Add(new PatientDto(p.PatientID, p.PatientFirstName, p.PatientLastName, p.PatientPhoneNumber, p.PatientEmail, p.PatientGender)));
            return Ok(PatientDtos);
        }

        /// <summary>
        /// Return all patients
        /// </summary>
        /// <returns>
        /// HEADER: 200(OK)
        /// CONTENT: All patients in the database
        /// </returns>
        /// <example>
        /// GET: api/PatientData/GetPatients
        /// </example>
        public IHttpActionResult GetPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();
            Patients.ForEach(p => PatientDtos.Add(new PatientDto(p.PatientID, p.PatientFirstName, p.PatientLastName, p.PatientPhoneNumber, p.PatientEmail, p.PatientGender)));
            return Ok(PatientDtos);
        }

        /// <summary>
        /// Find a patient by patient id.
        /// </summary>
        /// <param name="id">Represents the patient id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A patient in the system matching up to the patient id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/PatientData/FindPatientById/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult FindPatientById(int id)
        {
            Patient Patient = db.Patients.Find(id);
            if (Patient == null)
            {
                return NotFound();
            }
            PatientDto PatientDto = new PatientDto(
                Patient.PatientID, Patient.PatientFirstName,
                Patient.PatientLastName, Patient.PatientPhoneNumber,
                Patient.PatientEmail, Patient.PatientGender
                );

            return Ok(PatientDto);
        }

        /// <summary>
        /// Update an existing patient in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the patient id primary key</param>
        /// <param name="patient">JSON FORM data of a patient</param>
        /// <returns>
        /// HEADER: 204 (Success, no content response)
        /// or
        /// HEADER: 400 (Bad request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/UpdatePatient/1
        /// FORM DATA: Patient JSON object.
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        /// Adds a patient to the system
        /// </summary>
        /// <param name="Patient">JSON form data of a patient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Patient id, patient data.
        /// or
        /// HEADER: 400 (Bad request)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/AddPatient
        /// FORM DATA: Patient JSON object.
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult AddPatient(Patient Patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(Patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Patient.PatientID }, Patient);
        }

        /// <summary>
        /// delete a particular patient in the system
        /// </summary>
        /// <param name="id">Represents the patient id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 400 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/DeletePatient/1
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}