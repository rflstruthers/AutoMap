using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using WebApplication2.DAL;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using System.EnterpriseServices;

namespace WebApplication2.Controllers
{
    public class CompanyController : Controller
    {
        // Must enter Google Maps API Key here:
        readonly string apiKey = "AIzaSyAlNR7AYiRN1PF6p3tf9q7B28fk5d8Dx6o";
        readonly AutoMapContext db = new AutoMapContext();

        public async Task<ActionResult> ReadPolygons(int id)
        {
            AuthorizationRequest user = (from l in db.AuthorizationRequests
                         where l.Login != null
                         select l).FirstOrDefault();
            
            using (var client = new HttpClient())
            {
                ReadPolygonRequest polygonRequest = new ReadPolygonRequest
                {
                    Authorization = user,
                    CompanyIDs = new List<int> { id }
                };
                //REST service url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = await client.PostAsJsonAsync("geofence/readpolygons", polygonRequest);
                //response
                string responseString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseString);
                //Map response to ReadPolygonResponse model
                ReadPolygonResponse polygonResponse = JsonConvert.DeserializeObject<ReadPolygonResponse>(responseString);
                ViewBag.companyId = id;
                TempData["companyId"] = id;
                return View(polygonResponse);

            } 
        }

        public ActionResult ViewPolygon(int companyId, int polyId, string polyName, string polyPoints)
        {
            List<Point> points = System.Web.Helpers.Json.Decode<List<Point>>(polyPoints);
            Polygon polygon = new Polygon
            {
                PolygonID = polyId,
                PolygonName = polyName,
                Points = points
            };
            ViewBag.companyId = companyId;
            ViewBag.apiKey = apiKey;
            return View(polygon);
        }

        // Sends updatepolygons rest call
        // method is called on a click event in ViewPolygon.cshtml
        public async Task<ActionResult> UpdatePolygon(int polyId, string polyName, string polyPoints)
        {
            using (var client = new HttpClient())
            {
                // [Route("geofence/updatepolygons")]
                // UpdatePolygonResponse Post(UpdatePolygonRequest query);

                // format polyPoints correctly
                polyPoints = polyPoints.Replace("lat", "Latitude").Replace("lng", "Longitude");
                List<Point> points = System.Web.Helpers.Json.Decode<List<Point>>(polyPoints);
                Polygon polygon = new Polygon
                {
                    PolygonID = polyId,
                    PolygonName = polyName,
                    Points = points
                };

                AuthorizationRequest user = (from l in db.AuthorizationRequests
                                             where l.Login != null
                                             select l).FirstOrDefault();

                UpdatePolygonRequest polygonRequest = new UpdatePolygonRequest
                {
                    Authorization = user,
                    Polygons = new List<Polygon> { polygon }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //updatepolygons REST call
                var response = await client.PostAsJsonAsync("geofence/updatepolygons", polygonRequest);
                //response
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                //Map response to UpdatePolygonResponse model
                UpdatePolygonResponse polygonResponse = JsonConvert.DeserializeObject<UpdatePolygonResponse>(responseString);
                if (polygonResponse.Status == "SUCCESS")
                {
                    TempData["updateMessage"] = "Area has been successfully updated.";
                }
                else
                {
                    TempData["updateMessage"] = "An error was encountered while attempting to update area. Please try again later or contact the site administrator.";
                }
                return Json(Url.Action("ReadPolygons", "Company", new { id = TempData["companyId"] }));
            }
        }

        
        public ActionResult CreatePolygon(int companyId)
        {
            ViewBag.companyId = companyId;
            ViewBag.apiKey = apiKey;
            return View();
        }

        
        public async Task<ActionResult> SaveCreatedPolygon(int companyId, string polyName, string polyPoints)
        {
            using (var client = new HttpClient())
            {
                // [Route("geofence/createpolygons")]
                // CreatePolygonResponse Post(CreatePolygonRequest query);

                // format polyPoints correctly
                polyPoints = polyPoints.Replace("lat", "Latitude").Replace("lng", "Longitude");
                List<Point> points = System.Web.Helpers.Json.Decode<List<Point>>(polyPoints);
                Polygon polygon = new Polygon
                {
                    PolygonName = polyName,
                    Points = points
                };

                AuthorizationRequest user = (from l in db.AuthorizationRequests
                                             where l.Login != null
                                             select l).FirstOrDefault();
                CreatePolygonRequest polygonRequest = new CreatePolygonRequest
                {
                    Authorization = user,
                    CompanyID = companyId,
                    Polygons = new List<Polygon> { polygon }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = await client.PostAsJsonAsync("geofence/createpolygons", polygonRequest);
                //response
                string responseString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseString);
                //Map response to CreatePolygonResponse model
                CreatePolygonResponse polygonResponse = JsonConvert.DeserializeObject<CreatePolygonResponse>(responseString);
                if (polygonResponse.Status == "SUCCESS")
                {
                    TempData["createMessage"] = "Area has been successfully created.";
                }
                else
                {
                    TempData["createMessage"] = "An error was encountered while attempting to create area. Please try again later or contact the site administrator.";
                }
                return Json(Url.Action("ReadPolygons", "Company", new { id = TempData["companyId"] }));
            }
        }

        public async Task<ActionResult> DeletePolygon(int polyId)
        {
            using (var client = new HttpClient())
            {
                // [Route("geofence/deletepolygons")]
                // DeletePolygonResponse Post(DeletePolygonRequest query);
                AuthorizationRequest user = (from l in db.AuthorizationRequests
                                             where l.Login != null
                                             select l).FirstOrDefault();
                DeletePolygonRequest polygonRequest = new DeletePolygonRequest
                {
                    Authorization = user,
                    PolygonIDs = new List<int?> { polyId }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = await client.PostAsJsonAsync("geofence/deletepolygons", polygonRequest);
                //response
                string responseString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseString);
                //Map response to DeletePolygonResponse model
                DeletePolygonResponse polygonResponse = JsonConvert.DeserializeObject<DeletePolygonResponse>(responseString);
                if (polygonResponse.Status == "SUCCESS")
                {
                    TempData["deleteMessage"]  = "Area has been successfully deleted.";
                }
                else
                {
                    TempData["deleteMessage"]  = "An error was encountered while attempting to delete area. Please try again later or contact the site administrator.";
                }
                return Json(Url.Action("ReadPolygons", "Company", new { id = TempData["companyId"] }));
            }
        }

    }
}