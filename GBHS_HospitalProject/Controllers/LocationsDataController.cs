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
    public class LocationsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// return all locations in the db
        /// </summary>
        /// <returns>list of all locations</returns>
        // GET: api/LocationsData/ListLocations
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult ListLocations()
        {
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> locationDtos = new List<LocationDto>();

            Locations.ForEach(l => locationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(locationDtos);
        }
        /// <summary>
        /// returns list of locations for a particular service
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>list of locations linked with the particular service</returns>
        /// GET: api/LocationsData/ListLocationsforservice/{id}
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult ListLocationsforService (int id)
        {
            List<Location> Locations = db.Locations.Where(
                    l => l.Services.Any(
                        s => s.ServiceID == id)).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(LocationDtos);
        }
        /// <summary>
        /// list of locations without the particular serviceid
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>list of locations not linked with a particular service</returns>
        /// GET: api/LocationsData/ListLocationsWithoutService/{id}
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult ListLocationsWithoutService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                    l => !l.Services.Any(
                        s => s.ServiceID == id)).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(LocationDtos);
        }
        /// <summary>
        /// returns details for a particular location
        /// </summary>
        /// <param name="id">location id</param>
        /// <returns>details for a particular location</returns>
        // GET: api/LocationsData/findlocation/{id]
        [ResponseType(typeof(LocationDto))]
        [HttpGet]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Locations.Find(id);
            LocationDto LocationDto = new LocationDto()
            {
                LocationID = location.LocationID,
                LocationName = location.LocationName,
                LocationPhone = location.LocationPhone,
                LocationEmail = location.LocationEmail,
                LocationAddress = location.LocationAddress,
                LocationHasPic = location.LocationHasPic,
                PicExtension = location.PicExtension,
                Services = location.Services
            };
            if (location == null)
            {
                return NotFound();
            }

            return Ok(LocationDto);
        }
        /// <summary>
        /// updates a particular location information
        /// </summary>
        /// <param name="id">location id</param>
        /// <param name="location">location information(object)</param>
        /// <returns>updates the particular location information in the database</returns>
        // POST: api/LocationsData/updatelocation/{id}/{location}
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationID)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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
        /// adds a new location to the database
        /// </summary>
        /// <param name="location">location object</param>
        /// <returns>adds a new row of data in the locations table. creates a new location</returns>
        // POST: api/LocationsData/addlocation
        [ResponseType(typeof(Location))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationID }, location);
        }
        /// <summary>
        /// deletes a location given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/LocationsData/deletelocation/5
        [ResponseType(typeof(Location))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.LocationID == id) > 0;
        }
    }
}