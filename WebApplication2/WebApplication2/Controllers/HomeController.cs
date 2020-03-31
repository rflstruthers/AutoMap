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

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var client = new HttpClient())
            {
                //Login details
                AuthorizationRequest p = new AuthorizationRequest { Login = "rflstruthers@gmail.com", Password = "Putters1225!", RequestingAppName = "", RequestingAppVersion = "" };
                //REST servise url
                client.BaseAddress = new Uri("https://scott.lotlocate.com/REST/");
                //readcompanies REST call
                var response = client.PostAsJsonAsync("geofence/readcompanies", p).Result;
                //response
                string responseString = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(responseString);
                //// Response: { "Status":"SUCCESS","Companies":[{"CompanyID":1040000,"CompanyName":"Bob Brown Chevy"}
                //Map response to ReadCompaniesResponse model
                ReadCompaniesResponse companyResponse = JsonConvert.DeserializeObject<ReadCompaniesResponse>(responseString);

                return View(companyResponse);
            }
        }
    }
}
    