
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SensorServer;

namespace Sensors.Server.Db {
    public class Sensor {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Sensor))]
        public long ToSensorId { get; set; }
        public int Ip { get; set; }
        public int Port { get; set; }
        public long Longditude { get; set; }
        public long Latitude { get; set; }
        public DateTime LastSeen { get; set; }
        public ICollection<Reading>? Readings { get; set; }
        public Sensor? ToSensor { get; set; }

        public ICollection<Sensor>? FromSensors {get; set;}

    }
    public class Reading {
        [Key]
        public long RedingId { get; set;}
        public long Temperature { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        public long CO { get; set; }
        public long NO2 { get; set; }
        public long SO2 { get; set; }
        [ForeignKey(nameof(Sensor))]
        public long SensorId { get; set; }

        public Sensor? Sensor { get; set; }
    }

    public class SensorsDatabase : DbContext {
        private readonly ServerConfiguration _configuration;

        public DbSet<Sensor>? Sensors { get; set; }
        public DbSet<Reading>? Readings { get; set; }


        public SensorsDatabase(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=d.db");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Sensor>().HasOne<Sensor>().WithMany(i => i.FromSensors)
                .HasForeignKey(i => i.ToSensorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Sensor>().HasMany<Reading>().WithOne(i => i.Sensor)
                .HasForeignKey(i => i.SensorId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public Task<
    }
}