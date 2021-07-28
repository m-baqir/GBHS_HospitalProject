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
    public class ServicesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// lists all services in the database
        /// </summary>
        /// <returns>returns an ienumerable list of services</returns>
        [HttpGet]
        public IEnumerable<ServiceDto> ListServices()
        {
            List<Service> Services = db.Services.ToList();
            List<ServiceDto> ServiceDtos = new List<ServiceDto>();
            Services.ForEach(a => ServiceDtos.Add(new ServiceDto()
            {
                ServiceID = a.ServiceID,
                ServiceName = a.ServiceName,
                ServicePhone = a.ServicePhone,
                ServiceEmail = a.ServiceEmail,
                ServiceLocation = a.ServiceLocation,
                ServiceInfo = a.ServiceInfo,
                ServiceHasPic = a.ServiceHasPic,
                PicExtension = a.PicExtension,
                Locations = a.Locations
            }));
            return ServiceDtos;
        }

        /// <summary>
        /// finds a specific service given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns details of a particular service</returns>
        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult FindService(int id)
        {
            Service service = db.Services.Find(id);
            ServiceDto ServiceDto = new ServiceDto()
            {
                ServiceID = service.ServiceID,
                ServiceName = service.ServiceName,
                ServicePhone = service.ServicePhone,
                ServiceEmail = service.ServiceEmail,
                ServiceLocation = service.ServiceLocation,
                ServiceInfo = service.ServiceInfo,
                ServiceHasPic = service.ServiceHasPic,
                PicExtension = service.PicExtension,
                Locations = service.Locations
            };
            if (service == null)
            {
                return NotFound();
            }

            return Ok(ServiceDto);
        }
        /// <summary>
        /// provides the update functionality for a specific service given its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        // PUT: api/ServicesData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceID)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
        /// creates a new service in the db
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        // POST: api/ServicesData
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult AddService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceID }, service);
        }
        /// <summary>
        /// deletes a particular service given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ServicesData/5
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceID == id) > 0;
        }
    }
}