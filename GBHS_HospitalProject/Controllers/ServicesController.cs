using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using GBHS_HospitalProject.Models;
using GBHS_HospitalProject.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace GBHS_HospitalProject.Controllers
{
    public class ServicesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ServicesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44389/api/");
        }
        /// <summary>
        /// this method lists all services in the database
        /// </summary>
        /// <returns>returns list of servicedtos</returns>
        /// GET: services/list
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult List()
        {
            string url = "servicesdata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> Services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(Services);
        }
        /// <summary>
        /// this method returns the specific details of a service given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns servicedto object to the view</returns>
        // GET: Services/Details/5
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Details(int id)
        {
            DetailsService ViewModel = new DetailsService();

            string url = "servicesdata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedservice = response.Content.ReadAsAsync<ServiceDto>().Result;
            ViewModel.SelectedService = selectedservice;

            //show locations with this service
            url = "locationsdata/listlocationsforservice/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationsWithService = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.LocationsWithService = LocationsWithService;

            //show locations without this service
            url = "locationsdata/listlocationswithoutservice/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationsWithoutService = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.LocationsWithoutService = LocationsWithoutService;

            return View(ViewModel);
        }
        /// <summary>
        /// an empty method to route errors through
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// Associate method to link a service and location together in the bridge table
        /// </summary>
        /// <param name="id">service id</param>
        /// <param name="locationid">location id</param>
        /// <returns></returns>
        /// POST: services/associate/{id}/{locationid}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Associate(int id, int locationid)
        {
            string url = "servicesdata/associateservicewithlocation/" + id+"/"+locationid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }
        /// <summary>
        /// removes the link between a service and location in the bridge table.
        /// TODO: currently running into an error with this method. the method gets the call but does not update the relationship in the table.
        /// </summary>
        /// <param name="id">serviceid</param>
        /// <param name="locationid">locationid</param>
        /// <returns></returns>
        /// POST: services/unassocaite/{id}/{locationid}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UnAssociate(int id, int locationid)
        {
            string url = "servicesdata/unassociateservicewithlocation/" + id + "/" + locationid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }
        /// <summary>
        /// this method presents the form elements tot he user to create a new service in the database
        /// </summary>
        /// <returns></returns>
        // GET: Services/NEW
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            string url = "locationsdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            return View(LocationOptions);
        }
        /// <summary>
        /// creates a new service in the database
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        // POST: Services/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Service service)
        {
            string url = "servicesdata/addservice";
            string jsonpayload = jss.Serialize(service);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// presents specific service information in form elements before updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Services/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdateService ViewModel = new UpdateService();

            //service information
            string url = "servicesdata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedservice = response.Content.ReadAsAsync<ServiceDto>().Result;
            ViewModel.SelectedService = selectedservice;

            //list of locations to choose from
            url = "locationsdata/listlocations";
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            ViewModel.LocationOptions = LocationOptions;
            
            return View(ViewModel);
        }
        /// <summary>
        /// performs the actual update command in the db given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        // POST: Services/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Service service)
        {
            string url = "servicesdata/updateservice/" + id;
            string jsonpayload = jss.Serialize(service);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// leads to the delete confirm page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Services/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "servicesdata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedservice = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(selectedservice);
        }
        /// <summary>
        /// deletes a specific service given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Services/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "servicesdata/deleteservice/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
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
