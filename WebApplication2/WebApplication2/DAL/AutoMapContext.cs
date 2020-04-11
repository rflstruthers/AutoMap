
using WebApplication2.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

namespace WebApplication2.DAL
{
    public class AutoMapContext : DbContext
    {

        public AutoMapContext() : base("AutoMapContext")
        {
        }

        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}