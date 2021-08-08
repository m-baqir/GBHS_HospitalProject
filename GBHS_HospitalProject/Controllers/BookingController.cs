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
using Microsoft.AspNet.Identity;

namespace GBHS_HospitalProject.Controllers
{
    public class BookingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();

        static BookingController()
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

        // GET: Booking
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult List()
        {
            GetApplicationCookie();
            string url = "bookingdata/listbookings";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingDto> bookingDtos = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
            return View(bookingDtos);
        }

        [Authorize(Roles = "Admin,Guest")]
        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "bookingdata/findbookingbyid/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            //Booking booking = db.Bookings.Find(id);
            if (selectedBooking == null)
            {
                return HttpNotFound();
            }
            return View(selectedBooking);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,BookingStartTime,BookingEndTime,BookingReasonToVisit")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                string url = "bookingdata/addBooking";
                string jsonpayload = jss.Serialize(booking);

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
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "bookingdata/findbookingbyid/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            //Booking booking = db.Bookings.Find(id);
            if (selectedBooking == null)
            {
                return HttpNotFound();
            }
            return View(selectedBooking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,BookingStartTime,BookingEndTime,BookingReasonToVist")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                string url = "bookingdata/updatebooking/" + booking.BookingID;
                string jsonpayload = jss.Serialize(booking);
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
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "bookingdata/findbookingbyid/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBookingDto = response.Content.ReadAsAsync<BookingDto>().Result;
            //Booking booking = db.Bookings.Find(id);
            if (selectedBookingDto == null)
            {
                return HttpNotFound();
            }
            return View(selectedBookingDto);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "bookingdata/deletebooking/" + id;
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
            

        }

        public ActionResult Book(int? id)
        {
            //TODO
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "bookingdata/findbookingbyid/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            //Booking booking = db.Bookings.Find(id);
            if (selectedBooking == null) 
            {
                return HttpNotFound();
            }
            return View(selectedBooking);
        }

        [HttpPost]
        public ActionResult Book([Bind(Include = "BookingID,BookingID,BookingStartTime,BookingEndTime,BookingReasonToVisit")] Booking booking)
        {
            string url;
            HttpResponseMessage response;
            int patientId = 0;
            if (User != null && User.IsInRole("Guest"))
            {
                url = "patientData/FindPatientByUserId";
                response = client.GetAsync(url).Result;
                PatientDto patientDto = response.Content.ReadAsAsync<PatientDto>().Result;
                patientId = patientDto.PatientID;
                booking.UserID = User.Identity.GetUserId();
                booking.PatientID = patientId;
            }

            url = "bookingData/bookappointmentforpatient/" + booking.BookingID;
            
            
            string jsonPayload = jss.Serialize(booking);
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            response = client.PostAsync(url, content).Result;
            return RedirectToAction("List");
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
