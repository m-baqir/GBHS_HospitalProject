using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GBHS_HospitalProject.Models;
using GBHS_HospitalProject.Models.ViewModels;

namespace GBHS_HospitalProject.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        ApplicationDbContext db = new ApplicationDbContext();
        
        static PatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44389/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //Collect token as it is submitted to the controller
            //use it to pass along to the WebAPI
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);
            return;
        }

        // GET: Patient
        [Authorize(Roles ="Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();

            string Url = "PatientData/listPatients";
            HttpResponseMessage Response = client.GetAsync(Url).Result;

            IEnumerable<PatientDto> BookingDtos = Response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            return View(BookingDtos);
        }

        // GET: Patient/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            DetailsPatient ViewModel = new DetailsPatient();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "PatientData/FindPatientById/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedPatientDto = response.Content.ReadAsAsync<PatientDto>().Result;
            ViewModel.SelectedPatient = selectedPatientDto;

            url = "BookingData/FindBookingsByPatientId/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookingDto> patientBookingDtos = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            ViewModel.BookingsOfPatient = patientBookingDtos;
            
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,PatientFirstName,PatientLastName,PatientEmail,PatientPhoneNumber,PatientGender")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                string url = "PatientData/AddPatient";
                string jsonpayload = jss.Serialize(patient);

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

            return RedirectToAction("List");
            //return View(patient);
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string url = "patientdata/FindPatientById/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatientDto = response.Content.ReadAsAsync<PatientDto>().Result;

            if (SelectedPatientDto == null)
            {
                return HttpNotFound();
            }
            return View(SelectedPatientDto);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,PatientFirstName,PatientLastName,PatientEmail,PatientPhoneNumber,PatientGender")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                string url = "patientdata/updatepatient/" + patient.PatientID;
                string jsspayload = jss.Serialize(patient);
                HttpContent content = new StringContent(jsspayload);

                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response =  client.PostAsync(url, content).Result;
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }

                /*db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");*/
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        // GET: Patient/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "patientdata/findpatientbyid/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            //Patient patient = db.Patients.Find(id);
            if (selectedPatient == null)
            {
                return HttpNotFound();
            }
            return View(selectedPatient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "patientdata/deletepatient/" + id;
            HttpContent content = new StringContent("");
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
            /*Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
            */
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
