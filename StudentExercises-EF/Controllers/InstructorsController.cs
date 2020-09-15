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
    public class InstructorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Instructor.Include(i => i.Cohort);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(i => i.Cohort)
                .Include(s => s.AssignedStudents)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name");

            InstructorCohortsViewModel viewModel = new InstructorCohortsViewModel();


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

            // POST: Instructors/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,SlackHandle,Specialty,CohortId")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", instructor.CohortId);

            InstructorCohortsViewModel viewModel = new InstructorCohortsViewModel();
            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", instructor.CohortId);

            InstructorCohortsViewModel viewModel = new InstructorCohortsViewModel();

            viewModel.instructor = await _context.Instructor
                .FirstOrDefaultAsync(i => i.Id == id);

            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,SlackHandle,Specialty,CohortId")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Name", instructor.CohortId);

            InstructorCohortsViewModel viewModel = new InstructorCohortsViewModel();

            viewModel.instructor = await _context.Instructor
                .FirstOrDefaultAsync(i => i.Id == id);

            viewModel.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(viewModel);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructor
                .Include(i => i.Cohort)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructor.FindAsync(id);
            _context.Instructor.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructor.Any(e => e.Id == id);
        }
    }
}
