using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SchoolMS.DAL
{
    public partial class SchoolMSContext : DbContext
    {
        public SchoolMSContext()
            : base("name=SchoolMSContext")
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<LoginData> LoginData { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentCourse> StudentCourse { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<StudentCourseData_temp> StudentCourseData_temp { get; set; }
        public virtual DbSet<StudentCourseData_temp2> StudentCourseData_temp2 { get; set; }
        public virtual DbSet<StudentCourseData_temp3> StudentCourseData_temp3 { get; set; }
        public virtual DbSet<StudentCourseData_temp4> StudentCourseData_temp4 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.StudentCourse)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoginData>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<LoginData>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<School>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<School>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<School>()
                .HasMany(e => e.Student)
                .WithRequired(e => e.School)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Average)
                .HasPrecision(3, 2);

            modelBuilder.Entity<Student>()
                .HasOptional(e => e.LoginData)
                .WithRequired(e => e.Student);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudentCourse)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentCourseData_temp>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourseData_temp>()
                .Property(e => e.AverageGrade)
                .HasPrecision(10, 2);

            modelBuilder.Entity<StudentCourseData_temp2>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourseData_temp2>()
                .Property(e => e.AverageGrade)
                .HasPrecision(10, 2);

            modelBuilder.Entity<StudentCourseData_temp3>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourseData_temp3>()
                .Property(e => e.AverageGrade)
                .HasPrecision(38, 6);

            modelBuilder.Entity<StudentCourseData_temp4>()
                .Property(e => e.AverageGrade)
                .HasPrecision(38, 6);
        }
    }
}
