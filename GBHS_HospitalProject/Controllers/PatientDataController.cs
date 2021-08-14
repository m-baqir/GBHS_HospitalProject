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
using Microsoft.AspNet.Identity;
using System.Diagnostics;


namespace GBHS_HospitalProject.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult ListPatients()
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
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult FindPatientById(string id)
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
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult FindPatientByUserId()
        {
            string id = User.Identity.GetUserId();
            Patient Patient = db.Patients.Where(p=> p.PatientID == id).FirstOrDefault();
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
        /// Find Patients by prefix of firstname, lastname, email, or phone number
        /// </summary>
        /// <param name="prefix">represents prefix of firstname, lastname, email, or phone number</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: list of patients in the system
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/PatientData/FindPatientsByPrefix/John
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        [Authorize(Roles = "Admin")]
        [Route("Api/PatientData/FindPatientsByPrefix/{prefix}")]
        public IHttpActionResult FindPatientsByPrefix(string prefix)
        {
            List<Patient> patients = db.Patients.Where(p => p.PatientFirstName.StartsWith(prefix)
                                                || p.PatientLastName.StartsWith(prefix)
                                                || p.PatientEmail.StartsWith(prefix)
                                                || p.PatientPhoneNumber.StartsWith(prefix)).ToList();
            List<PatientDto> patientDtos = new List<PatientDto>();
            patients.ForEach(p => patientDtos.Add(new PatientDto(p.PatientID, p.PatientFirstName, p.PatientLastName, p.PatientPhoneNumber, p.PatientEmail, p.PatientGender)));
            return Ok(patientDtos);
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
        [Authorize(Roles ="Admin,Guest")]
        public IHttpActionResult UpdatePatient(string id, Patient patient)
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
        [Authorize(Roles="Admin,Guest")]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult AddPatient(Patient Patient)
        {
            //TODO: for guest, if
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //registered user is not admin.
            Debug.WriteLine("PatientDataController User.Identity.GetUserId() :" + User.Identity.GetUserId());
            if (User != null && !User.IsInRole("Admin"))
            {
                Patient.PatientID = User.Identity.GetUserId();
            }
            Patient.PatientBookings = new List<Booking>();
            db.Patients.Add(Patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = 0/*Patient.PatientID*/ }, Patient);
        }

        /// <summary>
        /// delete a particular patient in the system
        /// </summary>
        /// <param name="id">Represents the patient id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/DeletePatient/1
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(string id)
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

        public Patient GetPatientById(string id)
        {
            return db.Patients.Find(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(string id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}