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
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        /// <summary>
        /// list of locations
        /// </summary>
        /// <returns></returns>
        // GET: Location
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
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Location/Details/5
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

            return View(ViewModel);
        }
        public ActionResult Error()
        {

            return View();
        }

        // GET: Location/Create
        public ActionResult New()
        {
            string url = "servicessdata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServiceOptions = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            return View(ServiceOptions);           
        }

        // POST: Location/Create
        [HttpPost]
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

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateLocation ViewModel = new UpdateLocation();
            //current location
            string url = "locationsdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            ViewModel.SelectedLocation = SelectedLocation;
            //list of services
            url = "servicessdata/listservices";
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ServiceOptions = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;
            ViewModel.ServicesOptions = ServiceOptions;

            return View(ViewModel);
        }

        // POST: Location/Edit/5
        [HttpPost]
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

        // GET: Location/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "locationsdata/findlocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
            return View(SelectedLocation);
        }

        // POST: Location/Delete/5
        [HttpPost]
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
