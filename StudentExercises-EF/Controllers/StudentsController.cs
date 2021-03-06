﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercises_EF.Data;
using StudentExercises_EF.Models;
using StudentExercises_EF.Models.ViewModels;

namespace StudentExercises_EF.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Student.Include(s => s.Cohort);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var students = from s in _context.Student.Include(s => s.Cohort)
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                //case "Date":
                //    students = students.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "date_desc":
                //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Cohort)
                .Include(ae => ae.AssignedExercises)
                .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(m => m.Id == id);

           if (student == null)
            {
                return NotFound();
            }
                                 
            
            

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name");

            StudentCohortsViewModel viewModel = new StudentCohortsViewModel();

            
            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            //Inserting to the top of the list (index 0)
            viewModel.Cohorts.Insert(0, new SelectListItem() { Value = "0", Text = "Please Choose a Cohort" });

            return View(viewModel);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,SlackHandle,CohortId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", student.CohortId);
            StudentCohortsViewModel viewModel = new StudentCohortsViewModel();

            //viewModel.student = await _context.Student
            //    .FirstOrDefaultAsync(s => s.Id == id);

            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", student.CohortId);

            StudentCohortsViewModel viewModel = new StudentCohortsViewModel();

            viewModel.student = await _context.Student
                .FirstOrDefaultAsync(s => s.Id == id);

            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,SlackHandle,CohortId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", student.CohortId);
            StudentCohortsViewModel viewModel = new StudentCohortsViewModel();

            viewModel.student = await _context.Student
                .FirstOrDefaultAsync(s => s.Id == id);

            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Cohort)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
