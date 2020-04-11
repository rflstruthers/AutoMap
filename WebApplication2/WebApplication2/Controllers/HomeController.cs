using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;
using WebApplication2.DAL;
using System.Data.Entity;
using System.Net;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        //private AutoMapContext db = new AutoMapContext();

        public ActionResult Index()
        {
            using (AutoMapContext db = new AutoMapContext())
            {
                db.AuthorizationRequests.RemoveRange(db.AuthorizationRequests.Where(x => x.Login != null));
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Login(string login, string password)
        {
            //if input is empty, direct to an error page
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (AutoMapContext db = new AutoMapContext())
                {
                    AuthorizationRequest credentials = new AuthorizationRequest
                    {
                        Login = login,
                        Password = password
                    };

                    //Add the above object to the DB
                    db.AuthorizationRequests.Add(credentials);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ReadCompanies");
        }

        public ActionResult ReadCompanies()
        {
            using (AutoMapContext db = new AutoMapContext())
            {
                AuthorizationRequest user = (from l in db.AuthorizationRequests
                                         where l.Login != null
                                         select l).FirstOrDefault();

                using (var client = new HttpClient())
                {
                    //REST servise url
                    client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                    //readcompanies REST call
                    var response = client.PostAsJsonAsync("geofence/readcompanies", user).Result;
                    //response
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(responseString);
                    //Map response to ReadCompaniesResponse model
                    ReadCompaniesResponse companyResponse = JsonConvert.DeserializeObject<ReadCompaniesResponse>(responseString);

                    return View(companyResponse);
                }
            }
        }
    }
}
    