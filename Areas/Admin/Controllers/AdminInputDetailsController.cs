using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Diagnostics;

namespace IS220_PROJECT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("[controller]/[action]")]
    public class AdminInputDetailsController : Controller
    {
        private readonly dbFrameContext _context;
        public INotyfService _notyfService { get; }

        public AdminInputDetailsController(dbFrameContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminInputDetails
        public async Task<IActionResult> Index(int? inputId)
        {
            if(inputId == null)
            {
                return NotFound();
            }
            var dbFrameContext = _context.InputDetails.Include(i => i.Input).Where(i => i.InputId == inputId);
            return View(await dbFrameContext.ToListAsync());
        }

        // GET: Admin/AdminInputDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InputDetails == null)
            {
                return NotFound();
            }

            var inputDetail = await _context.InputDetails
                .Include(i => i.Input)
                .FirstOrDefaultAsync(m => m.InputDetailId == id);
            if (inputDetail == null)
            {
                return NotFound();
            }

            return View(inputDetail);
        }

        // GET: Admin/AdminInputDetails/Create
        public IActionResult Create(int? id)
        {
            ViewBag.InputId = id;
            return View();
        }

        // POST: Admin/AdminInputDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("InputDetailId,InputId,ProductId,Quantity,InputPrice,Note")] InputDetail inputDetail)
        {
            if (id != inputDetail.InputId)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Add(inputDetail);
                await _context.SaveChangesAsync();
                string url = "/AdminInputDetails/Index?inputId=" + inputDetail.InputId.ToString();
                return Redirect(url);
            }
            ViewBag.InputId = id;
            return View(inputDetail);
        }

        // GET: Admin/AdminInputDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InputDetails == null)
            {
                return NotFound();
            }

            
            var inputDetail = _context.InputDetails.Include(p => p.Input).FirstOrDefault(p => p.InputDetailId == id);
            if (inputDetail == null)
            {
                return NotFound();
            }

            return View(inputDetail);
        }
        
        // POST: Admin/AdminInputDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InputId, InputDetailId, ProductId,Quantity,InputPrice,Note")] InputDetail inputDetail)
        {
            if (id != inputDetail.InputDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inputDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InputDetailExists(inputDetail.InputDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string url = "/AdminInputDetails/Index?inputId=" + inputDetail.InputId.ToString();
                return Redirect(url);
            }
            return View(inputDetail);
        }

        // GET: Admin/AdminInputDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InputDetails == null)
            {
                return NotFound();
            }

            var inputDetail = await _context.InputDetails
                .Include(i => i.Input)
                .FirstOrDefaultAsync(m => m.InputDetailId == id);
            if (inputDetail == null)
            {
                return NotFound();
            }

            return View(inputDetail);
        }

        // POST: Admin/AdminInputDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InputDetails == null)
            {
                return Problem("Entity set 'dbFrameContext.InputDetails'  is null.");
            }
            var inputDetail = await _context.InputDetails.FindAsync(id);
            var inputId = inputDetail.InputId;
            if (inputDetail != null)
            {
                _context.InputDetails.Remove(inputDetail);
            }
            
            await _context.SaveChangesAsync();
            string url = "/AdminInputDetails/Index?inputId=" + inputId.ToString();
            return Redirect(url);
        }

        private bool InputDetailExists(int id)
        {
          return _context.InputDetails.Any(e => e.InputDetailId == id);
        }
    }
}
