using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using UCRS.Model;

namespace UCRS.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
            : base("UCRSDb")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UniversityContext>()); 
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentInCourse> StudentsInCourses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<StudentInCourse>()
                .HasRequired(b => b.Course)
                .WithMany()
                .WillCascadeOnDelete(true);
        }
    }
}