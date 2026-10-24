using Microsoft.EntityFrameworkCore;

// TODO: put the models in their own files in a folder called Models.

namespace SeeSharp.Api
{
    public class SeeSharpContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Glasses> Glasses { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public string DbPath { get; } = "SeeSharp.db";
        public SeeSharpContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "SeeSharp.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Glasses> Glasses { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }

    public class Glasses
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }

    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}