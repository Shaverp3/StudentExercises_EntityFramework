using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises_EF.Models.ViewModels
{
    public class InstructorCohortsViewModel
    {

        public Instructor instructor { get; set; }

        // This will be our dropdown
        public List<SelectListItem> Cohorts { get; set; } = new List<SelectListItem>();
        
    }
}

