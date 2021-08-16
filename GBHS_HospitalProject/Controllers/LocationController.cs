using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using GBHS_HospitalProject.Models;
using GBHS_HospitalProject.Models.ViewModels;
using System.Web.Script.Serialization;

namespace GBHS_HospitalProject.Controllers
{   
    public class LocationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static LocationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44389/api/");
        }
        /// <summary>
        /// list of locations
        /// </summary>
        /// <returns>list of all locations in the db</returns>
        // GET: Location/list
        public ActionResult List()
        {
            string url = "locationsdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> Locations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            return View(Locations);
        }
        /// <summary>
        /// details page of a location
        /// </summary>
        /// <param name="id">location id</param>
        /// <returns>displays the details of a particular location given its id</returns>
        // GET: Location/Details/{id}
        public ActionResult Details(int id)
        {
            DetailsLocation ViewModel = new DetailsLocation();
            //get location details
            string url = "locationsdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            ViewModel.SelectedLocation = SelectedLocation;

            //get all services at this location
            url = "servicesdata/listservicesforlocation/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServicesAtLocation = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            ViewModel.ServicesAtLocation = ServicesAtLocation;

            //get all services not at this location
            url = "servicesdata/listservicesnotforlocation/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServicesNotAtLocation = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            ViewModel.ServicesNotAtLocation = ServicesNotAtLocation;

            return View(ViewModel);
        }
        public ActionResult Error()
        {

            return View();
        }
        /// <summary>
        /// presents the form elements to create a new location model
        /// </summary>
        /// <returns></returns>
        // GET: Location/NEW
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            string url = "servicesdata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServiceOptions = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(ServiceOptions);           
        }
        /// <summary>
        /// adds the new location object created in NEW into the database
        /// </summary>
        /// <param name="location">locationobject</param>
        /// <returns></returns>
        // POST: Location/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Location location)
        {
            string url = "locationsdata/addlocation";

            string jsonpayload = jss.Serialize(location);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // TODO: Add insert logic here

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// presents the edit form fields to update details of a particular location
        /// </summary>
        /// <param name="id">location id</param>
        /// <returns>new details of a location model</returns>
        // GET: Location/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateLocation ViewModel = new UpdateLocation();
            //current location
            string url = "locationsdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            ViewModel.SelectedLocation = SelectedLocation;
            //list of services
            url = "servicesdata/listservices";
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServiceOptions = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            ViewModel.ServicesOptions = ServiceOptions;

            return View(ViewModel);
        }
        /// <summary>
        /// updates the current location details with the new details given in the EDIT method
        /// </summary>
        /// <param name="id">location id</param>
        /// <param name="location">location model</param>
        /// <returns>updates the details in the database</returns>
        // POST: Location/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Location location)
        {
            string url = "locationsdata/updatelocation/" + id;
            string jsonpayload = jss.Serialize(location);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// presents a warning page before going ahead with deletion of a location
        /// </summary>
        /// <param name="id">location id</param>
        /// <returns></returns>
        // GET: Location/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "locationsdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            return View(SelectedLocation);
        }
        /// <summary> 
        /// deletes a particular location given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Location/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "locationsdata/deletelocation/" + id;
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
    }
}
