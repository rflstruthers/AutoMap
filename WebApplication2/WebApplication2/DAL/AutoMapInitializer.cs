
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class AutoMapInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AutoMapContext>
    {
        protected override void Seed(AutoMapContext context)
        {
            var AuthorizationRequests = new List<AuthorizationRequest>
            {
            new AuthorizationRequest {
                Login = "",
                Password = "",
                RequestingAppName = "",
                RequestingAppVersion = ""
            }
            };

            AuthorizationRequests.ForEach(s => context.AuthorizationRequests.Add(s));
            context.SaveChanges();
            
        }
    }
}