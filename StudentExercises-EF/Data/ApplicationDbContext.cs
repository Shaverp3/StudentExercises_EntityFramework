using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentExercises_EF.Models;

namespace StudentExercises_EF.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cohort> Cohort { get; set; }

        public DbSet<Exercise> Exercise { get; set; }

        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<Student> Student { get; set; }

        public DbSet<StudentJoinExercise> StudentJoinExercise { get; set; }
    }
}
