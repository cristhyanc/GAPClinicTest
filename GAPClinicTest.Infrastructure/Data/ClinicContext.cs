using GAPClinicTest.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Infrastructure.Data
{
   public class ClinicContext: DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options)
           : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient > Patients { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>();
            modelBuilder.Entity<Patient>();
           
        }
    }
}
