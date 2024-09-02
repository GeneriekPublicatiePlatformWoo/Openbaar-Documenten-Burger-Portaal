using Microsoft.EntityFrameworkCore;
using ODBP;

namespace ODBP.Data
{
    public class OdbpDbContext : DbContext
    {
        public OdbpDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<WeatherForecast>().HasData(
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Mild" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Warm" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), TemperatureC = 15, Summary = "Cool" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), TemperatureC = 30, Summary = "Hot" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), TemperatureC = 10, Summary = "Chilly" }
            );
        }
    }
}