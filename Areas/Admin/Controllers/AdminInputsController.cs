using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;
using PagedList.Core;

namespace IS220_PROJECT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("[controller]/[action]")]
    public class AdminInputsController : Controller
    {
        private readonly dbFrameContext _context;

        public AdminInputsController(dbFrameContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminInputs
        public async Task<IActionResult> Index(int? page, DateTime? startDate, DateTime? endDate)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Input> lsInv = new List<Input>();
            if (startDate != null && endDate != null)
            {
                lsInv = _context.Inputs.AsNoTracking().Where(i => i.Date.Value.Date <= endDate.Value.Date && i.Date.Value.Date >= startDate.Value.Date).OrderBy(i => i.Date).ToList();
            }
            else
                lsInv = _context.Inputs.AsNoTracking().OrderBy(i => i.InputId).ToList();
            PagedList<Input> models = new PagedList<Input>(lsInv.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.StartDate = startDate == null?"" : startDate.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = endDate == null ? "": endDate.Value.Date.ToString("dd/MM/yyyy");

            return View(models);
        }
        public async Task<IActionResult> Filter(DateTime? startDate, DateTime? endDate)
        {
            var url = "";
            if (startDate != null && endDate != null)
                url = $"/AdminInputs/Index?startDate={startDate}&endDate={endDate}";
            else
                url = "/AdminInputs/Index";

            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminInputs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inputs == null)
            {
                return NotFound();
            }

            var input = _context.Inputs.Join(
                                            _context.InputDetails,
                                            _input => _input.InputId,
                                            _inputD => _inputD.InputId,
                                            (_input, _inputD) => new Invoice
                                            {
                                                InputId = _input.InputId,
                                                InputDetailId = _inputD.InputDetailId,
                                                Date = _input.Date,
                                                ProductId = _inputD.ProductId,
                                                Quantity = _inputD.Quantity,
                                                InputPrice = _inputD.InputPrice,
                                                Note = _inputD.Note
                                            })
                                        .Join(
                                            _context.Products,
                                            _invoice => _invoice.ProductId,
                                            _products => _products.ProductId,
                                            (_invoice, _products) => new  Invoice
                                            {
                                                InputId = _invoice.InputId,
                                                InputDetailId = _invoice.InputDetailId,
                                                Date = _invoice.Date,
                                                ProductId = _invoice.ProductId,
                                                ProductName = _products.ProductName,
                                                Quantity = _invoice.Quantity,
                                                InputPrice = _invoice.InputPrice,
                                                Note = _invoice.Note
                                            }
                                         )
                                        .Where(i => i.InputId == id);
                //.FirstOrDefaultAsync(m => m.InputId == id);
            if (input == null)
            {
                return NotFound();
            }
            //Invoice _invoice = new Invoice(input.)
            return View(input);
        }

        // GET: Admin/AdminInputs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminInputs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InputId,Date")] Input input)
        {
            if (ModelState.IsValid)
            {
                _context.Add(input);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }

        // GET: Admin/AdminInputs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inputs == null)
            {
                return NotFound();
            }

            var input = await _context.Inputs.FindAsync(id);
            if (input == null)
            {
                return NotFound();
            }
            return View(input);
        }

        // POST: Admin/AdminInputs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InputId,Date")] Input input)
        {
            if (id != input.InputId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(input);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InputExists(input.InputId))
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
            return View(input);
        }

        // GET: Admin/AdminInputs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inputs == null)
            {
                return NotFound();
            }

            var input = await _context.Inputs
                .FirstOrDefaultAsync(m => m.InputId == id);
            if (input == null)
            {
                return NotFound();
            }

            return View(input);
        }

        // POST: Admin/AdminInputs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inputs == null)
            {
                return Problem("Entity set 'dbFrameContext.Inputs'  is null.");
            }
            var input = await _context.Inputs.FindAsync(id);
            if (input != null)
            {
                _context.Inputs.Remove(input);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InputExists(int id)
        {
          return _context.Inputs.Any(e => e.InputId == id);
        }
    }
}
