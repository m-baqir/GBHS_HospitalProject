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
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Get all bookings in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All bookings in the database
        /// </returns>
        /// <example>
        /// // GET: api/BookingData/ListBookings
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();
            Bookings.ForEach(b =>
            {
                if(b.Patient != null && b.Specialist != null)
                {
                    BookingDtos.Add(
            new BookingDto(b.BookingID, b.BookingStartTime, b.BookingEndTime, b.BookingReasonToVisit,
            b.Patient.PatientID, b.Patient.PatientFirstName, b.Patient.PatientLastName,
            b.Specialist.SpecialistID, b.Specialist.SpecialistFirstName, b.Specialist.SpecialistLastName));
                }
                else if(b.Patient == null && b.Specialist == null)
                {
                    if(b.Specialist != null)
                    {
                        BookingDtos.Add(new BookingDto(b.BookingID, b.BookingStartTime, b.BookingEndTime, b.BookingReasonToVisit, b.Specialist.SpecialistID, b.Specialist.SpecialistFirstName, b.Specialist.SpecialistLastName));
                    }
                    else
                    {
                        BookingDtos.Add(new BookingDto(b.BookingID, b.BookingStartTime, b.BookingEndTime, b.BookingReasonToVisit));
                    }
                    
                }
                
                
            }
            
            
        );

            return Ok(BookingDtos);
        }
        /// <summary>
        /// Find a particular Booking in the database by Booking ID
        /// </summary>
        /// <param name="id">Represents the booking id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A booking in the system matching up to the booking id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/BookingData/FindBookingById/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult FindBookingById(int id)
        {
            Booking booking = db.Bookings.Find(id);

            if (booking == null)
            {
                return NotFound();
            }
            BookingDto bookingDto = null;
            if(booking.Patient != null && booking.Specialist != null)
            {
                bookingDto = new BookingDto(
                booking.BookingID, booking.BookingStartTime,
                booking.BookingEndTime, booking.BookingReasonToVisit,
                (int)booking.PatientID, booking.Patient.PatientFirstName,
                booking.Patient.PatientLastName, (int)booking.SpecialistID,
                booking.Specialist.SpecialistFirstName, booking.Specialist.SpecialistLastName
                );
            }
            else if(booking.Patient == null)
            {
                if(booking.Specialist != null)
                {
                    bookingDto = new BookingDto(
                booking.BookingID, booking.BookingStartTime,
                booking.BookingEndTime, booking.BookingReasonToVisit,
                 booking.Specialist.SpecialistID,
                booking.Specialist.SpecialistFirstName, booking.Specialist.SpecialistLastName
                );
                }
                else
                {
                    bookingDto = new BookingDto(
                booking.BookingID, booking.BookingStartTime,
                booking.BookingEndTime, booking.BookingReasonToVisit);
                }
            }
            
            return Ok(bookingDto);
        }

        /// <summary>
        /// Find patient's Bookings in the database by patient ID
        /// </summary>
        /// <param name="id">Represents the patient id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: a list of bookings in the system matching up to the patient id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/BookingData/FindBookingsByPatientId/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult FindBookingsByPatientId(string id)
        {
            List<Booking> bookings = db.Bookings.Where(b => b.Patient.PatientID.ToString() == id).ToList();
            List<BookingDto> bookingDtos = new List<BookingDto>();
            if (bookings == null)
            {
                return NotFound();
            }
            bookings.ForEach(b => bookingDtos.Add(new BookingDto(
                b.BookingID, b.BookingStartTime, b.BookingEndTime, b.BookingReasonToVisit,
                (int)b.PatientID, b.Patient.PatientFirstName, b.Patient.PatientLastName,
                (int)b.SpecialistID, b.Specialist.SpecialistFirstName, b.Specialist.SpecialistLastName
                )));

            return Ok(bookingDtos);
        }

        /// <summary>
        /// Update a particular booking in the system with POST Data input.
        /// </summary>
        /// <param name="id">Represents the booking id primary key</param>
        /// <param name="booking">JSON FORM DATA of a booking</param>
        /// <returns>
        /// HEADER: 204 (Success, no content response)
        /// or
        /// HEADER: 400 (Bad request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/1
        /// FORM DATA: Booking JSON object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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
        /// Adds a booking in the system.
        /// </summary>
        /// <param name="booking">JSON FORM DATA of a booking</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// or
        /// HEADER: 400 (Bad request)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/AddBooking
        /// FORM Data: Booking JSON object.
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult AddBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.BookingID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingID }, booking);
        }

        /// <summary>
        /// Delete a particular booking in the database by booking id
        /// </summary>
        /// <param name="id">Represents booking id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (BAD REQUEST)
        /// </returns>
        /// <example>
        /// POST: api/BookingData/DeleteBooking/1
        /// FORM DATA: (empty)
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}