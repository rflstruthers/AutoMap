using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;

namespace WebApplication2.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult ReadPolygons(int id)
        {
            using (var client = new HttpClient())
            {
                // [Route("geofence/readpolygons")]
                // ReadPolygonResponse Post(ReadPolygonRequest query);
                
                ReadPolygonRequest polygonRequest = new ReadPolygonRequest
                {
                    Authorization = new AuthorizationRequest { Login = "rflstruthers@gmail.com", Password = "Putters1225!", RequestingAppName = "", RequestingAppVersion = "" },
                    CompanyIDs = new List<int> { id }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = client.PostAsJsonAsync("geofence/readpolygons", polygonRequest).Result;
                //response
                string responseString = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(responseString);
                //Map response to ReadPolygonResponse model
                ReadPolygonResponse polygonResponse = JsonConvert.DeserializeObject<ReadPolygonResponse>(responseString);
                ViewBag.companyId = id;
                TempData["companyId"] = id;
                return View(polygonResponse);

            } 
        }

        public ActionResult ViewPolygon(int polyId, string polyName, string polyPoints)
        {
            List<Point> points = System.Web.Helpers.Json.Decode<List<Point>>(polyPoints);
            Polygon polygon = new Polygon
            {
                PolygonID = polyId,
                PolygonName = polyName,
                Points = points
            };
            //ViewBag.Message = TempData["Message"];
            return View(polygon);
        }

        // Sends updatepolygons rest call
        // method is called on a click event in ViewPolygon.cshtml
        public ActionResult UpdatePolygon(int polyId, string polyName, string polyPoints)
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
                UpdatePolygonRequest polygonRequest = new UpdatePolygonRequest
                {
                    Authorization = new AuthorizationRequest { Login = "rflstruthers@gmail.com", Password = "Putters1225!", RequestingAppName = "", RequestingAppVersion = "" },
                    Polygons = new List<Polygon> { polygon }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //updatepolygons REST call
                var response = client.PostAsJsonAsync("geofence/updatepolygons", polygonRequest).Result;
                //response
                string responseString = response.Content.ReadAsStringAsync().Result;
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
            //List<int> Ids = System.Web.Helpers.Json.Decode<List<int>>(polyIds);
            //ViewBag.polyId = Ids.Max();
            ViewBag.companyId = companyId;
            return View();
        }

        
        public ActionResult SaveCreatedPolygon(int companyId, string polyName, string polyPoints)
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
                CreatePolygonRequest polygonRequest = new CreatePolygonRequest
                {
                    Authorization = new AuthorizationRequest { Login = "rflstruthers@gmail.com", Password = "Putters1225!", RequestingAppName = "", RequestingAppVersion = "" },
                    CompanyID = companyId,
                    Polygons = new List<Polygon> { polygon }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = client.PostAsJsonAsync("geofence/createpolygons", polygonRequest).Result;
                //response
                string responseString = response.Content.ReadAsStringAsync().Result;
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

        public ActionResult DeletePolygon(int polyId)
        {
            using (var client = new HttpClient())
            {
                // [Route("geofence/deletepolygons")]
                // DeletePolygonResponse Post(DeletePolygonRequest query);

                DeletePolygonRequest polygonRequest = new DeletePolygonRequest
                {
                    Authorization = new AuthorizationRequest { Login = "rflstruthers@gmail.com", Password = "Putters1225!", RequestingAppName = "", RequestingAppVersion = "" },
                    PolygonIDs = new List<int?> { polyId }
                };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = client.PostAsJsonAsync("geofence/deletepolygons", polygonRequest).Result;
                //response
                string responseString = response.Content.ReadAsStringAsync().Result;
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
                return RedirectToAction("ReadPolygons", new { id =  TempData["companyId"]});
                //return Json(Url.Action("Index", "Home"));
            }
        }

    }
}