using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GBHS_HospitalProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Patient Patient { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //Add Location entity to the system
        public DbSet<Location> Locations { get; set; }
        //Add Service entity to the system
        public DbSet<Service> Services { get; set; }
        //Add Department enity to the system
        public DbSet<Department> Departments { get; set; }
        //Add Specialist entity to the system
        public DbSet<Specialist> Specialists { get; set; }
        //Add Patient entity to the system
        public DbSet<Patient> Patients { get; set; }
        //Add Booking entity to the system
        public DbSet<Booking> Bookings { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}