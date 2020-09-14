using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises_EF.Models
{
    public class Cohort
    {

        public int Id { get; set; }

        [Required]
            [StringLength(11, MinimumLength = 5)]
            public string Name { get; set; }

            public List<Student> Students { get; set; } = new List<Student>();
            public List<Instructor> Instructors { get; set; } = new List<Instructor>();

    }
}

