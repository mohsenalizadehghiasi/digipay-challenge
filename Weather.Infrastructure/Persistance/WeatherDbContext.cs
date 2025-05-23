using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistance.Models;


namespace Weather.Infrastructure.Persistance
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }
        public DbSet<WeatherRecord> WeatherRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherRecord>(entity =>
            {
                entity.ToTable("WeatherRecords");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp)
                      .IsRequired();
                entity.Property(e => e.JsonPayload)
                      .IsRequired();
                entity.HasIndex(e => e.Timestamp);
            });
        }
    }
}
