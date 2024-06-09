using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;

namespace IS220_PROJECT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("[controller]/[action]")]
    public class AdminOrderDetailsController : Controller
    {
        private readonly dbFrameContext _context;

        public AdminOrderDetailsController(dbFrameContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrderDetails
        public async Task<IActionResult> Index(int? OrderId)
        {
            if (OrderId == null)
            {
                return NotFound();
            }
            var dbFrameContext = _context.OrderDetails.Include(o => o.Order).Include(o => o.Product).Where(o => o.OrderId == OrderId);
            return View(await dbFrameContext.ToListAsync());
        }

        // GET: Admin/AdminOrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Admin/AdminOrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Admin/AdminOrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,OrderId,ProductId,OrderNumber,Quantity,Discount,Total,ShipDate")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: Admin/AdminOrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: Admin/AdminOrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,OrderId,ProductId,OrderNumber,Quantity,Discount,Total,ShipDate")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: Admin/AdminOrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Admin/AdminOrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'dbFrameContext.OrderDetails'  is null.");
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}
