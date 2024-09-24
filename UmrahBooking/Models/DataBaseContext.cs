using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UmrahBooking.Models
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<Trip> Tips { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                // Add any additional configuration for the User entity here
            });

            // Other entity configurations go here
        }
        

    }
    public class Status
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
