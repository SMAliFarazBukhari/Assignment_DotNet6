using Assignment_DotNet6.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_DotNet6.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql(_ConnectionString,ServerVersion.AutoDetect(_ConnectionString));
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}
