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
using System.Diagnostics;
using System.Web;
using System.IO;

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

        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult ListServicesForLocation(int id)
        {
            //all services that match with locationid
            List<Service> services = db.Services.Where(
                s => s.Locations.Any(
                    l => l.LocationID == id
                )).ToList();
            List<ServiceDto> ServiceDtos = new List<ServiceDto>();

            services.ForEach(a => ServiceDtos.Add(new ServiceDto()
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

            return Ok(ServiceDtos);
        }

        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult ListServicesNotForLocation(int id)
        {
            //all services that do not match with locationid
            List<Service> services = db.Services.Where(
                s => !s.Locations.Any(
                    l => l.LocationID == id
                )).ToList();
            List<ServiceDto> ServiceDtos = new List<ServiceDto>();

            services.ForEach(a => ServiceDtos.Add(new ServiceDto()
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

            return Ok(ServiceDtos);
        }
        /// <summary>
        /// associate a service with a location given ids for both
        /// </summary>
        /// <param name="serviceid"></param>
        /// <param name="locationid"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("api/ServicesData/AssociateServiceWithLocation/{serviceid}/{locationid}")]
        public IHttpActionResult AssociateServiceWithLocation(int serviceid, int locationid)
        {
            Service SelectedService = db.Services.Include(a => a.Locations).Where(a => a.ServiceID == serviceid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if(SelectedService==null || SelectedLocation == null)
            {
                return NotFound();
            }

            SelectedService.Locations.Add(SelectedLocation);
            db.SaveChanges();
            return Ok();
        }
        /// <summary>
        /// removes an association between service and location given their ids
        /// </summary>
        /// <param name="serviceid"></param>
        /// <param name="locationid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ServiceData/UnAssociateServiceWithLocation/{serviceid}/{locationid}")]
        public IHttpActionResult UnAssociateServiceWithLocation(int serviceid, int locationid)
        {
            Service SelectedService = db.Services.Include(a => a.Locations).Where(a => a.ServiceID == serviceid).FirstOrDefault();
            Location SelectedLocation = db.Locations.Find(locationid);

            if (SelectedService == null || SelectedLocation == null)
            {
                return NotFound();
            }

            SelectedService.Locations.Remove(SelectedLocation);
            db.SaveChanges();

            return Ok();
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
            // Picture update is handled by another method
            db.Entry(service).Property(a => a.ServiceHasPic).IsModified = false;
            db.Entry(service).Property(a => a.PicExtension).IsModified = false;

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

        [HttpPost]
        public IHttpActionResult UploadServicePic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var servicePic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (servicePic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(servicePic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/images/service/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Services/"), fn);

                                //save the file
                                servicePic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the service haspic and picextension fields in the database
                                Service Selectedservice = db.Services.Find(id);
                                Selectedservice.ServiceHasPic = haspic;
                                Selectedservice.PicExtension = extension;
                                db.Entry(Selectedservice).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("service Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

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