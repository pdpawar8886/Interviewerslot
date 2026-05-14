using Interviewerslot.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace Interviewerslot.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor → connection string name
        public ApplicationDbContext() : base("InterviewDB")
        {
        }

        public DbSet<Sir> Sirs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SirAvailability> SirAvailabilities { get; set; }
        public DbSet<InterviewBooking> InterviewBookings { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Sir -> SirAvailability: disable cascade delete
            modelBuilder.Entity<SirAvailability>()
                .HasRequired(sa => sa.Sir)
                .WithMany()
                .HasForeignKey(sa => sa.SirId)
                .WillCascadeOnDelete(false);

            // Sir -> InterviewBooking: disable cascade delete
            modelBuilder.Entity<InterviewBooking>()
                .HasRequired(ib => ib.Sir)
                .WithMany()
                .HasForeignKey(ib => ib.SirId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InterviewBooking>() 
               .HasRequired(ib => ib.Student)
               .WithMany()
               .HasForeignKey(ib => ib.StudentId)
               .WillCascadeOnDelete(false);

            

            base.OnModelCreating(modelBuilder);
        }

    }
}
