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
    public class AdminOrdersController : Controller
    {
        private readonly dbFrameContext _context;

        public AdminOrdersController(dbFrameContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public async Task<IActionResult> Index(int? page, DateTime? startDate, DateTime? endDate)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Order> lsOrder = new List<Order>();
            if (startDate != null && endDate != null)
            {
                lsOrder = _context.Orders.AsNoTracking().Include(p => p.Payment).Include(p => p.TransactStatus).Include(p => p.Customer)
                    .Where(i => i.OrderDate.Value.Date <= endDate.Value.Date && i.OrderDate.Value.Date >= startDate.Value.Date)
                    .OrderBy(i => i.OrderDate)
                    .ToList();
            }
            else
                lsOrder = _context.Orders.AsNoTracking().Include(p => p.Payment).Include(p => p.TransactStatus).Include(p => p.Customer).OrderBy(i => i.OrderId).ToList();
            PagedList<Order> models = new PagedList<Order>(lsOrder.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.StartDate = startDate == null ? "" : startDate.Value.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = endDate == null ? "" : endDate.Value.Date.ToString("dd/MM/yyyy");

            return View(models);
        }
        public async Task<IActionResult> Filter(DateTime? startDate, DateTime? endDate)
        {
            var url = "";
            if (startDate != null && endDate != null)
                url = $"/AdminOrders/Index?startDate={startDate}&endDate={endDate}";
            else
                url = "/AdminOrders/Index";

            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = _context.OrderDetails.Join(
                                            _context.Orders,
                                            _orderD => _orderD.OrderId,
                                            _order => _order.OrderId,
                                             (_orderD, _order) => new
                                             {
                                                 OrderId = _order.OrderId,
                                                 OrdetailId = _orderD.OrderDetailId,
                                                 OrderDate = _order.OrderDate,
                                                 CustomerId = _order.CustomerId,
                                                 ProductId = _orderD.ProductId,
                                                 Quantity = _orderD.Quantity,
                                                 OrderNumber = _orderD.OrderNumber,
                                             })
                                        .Join(
                                            _context.Customers,
                                             _o => _o.CustomerId,
                                             _c => _c.CustomerId,
                                            (_o, _c) => new CustomerInvoice
                                            {
                                                OrderId = _o.OrderId,
                                                OrderDate = _o.OrderDate,
                                                ProductId = _o.ProductId,
                                                Quantity = _o.Quantity,
                                                OrderNumber = _o.OrderNumber,
                                                AccountId = _c.AccountId,
                                                CustomerId = _c.CustomerId,
                                                CustomerName = _c.FullName,
                                                Email = _c.Email,
                                                Phone = _c.Phone,
                                                Address = _c.Address
                                            })
                                        .Join(
                                            _context.Products,
                                            _invoice => _invoice.ProductId,
                                            _products => _products.ProductId,
                                            (_invoice, _products) => new CustomerInvoice
                                            {
                                                OrderId = _invoice.OrderId,
                                                OrderDetailId = _invoice.OrderDetailId,
                                                OrderDate = _invoice.OrderDate,
                                                ProductId = _invoice.ProductId,
                                                ProductName = _products.ProductName,
                                                Thumbnail = _products.Thumbnail,
                                                Quantity = _invoice.Quantity,
                                                Price = _products.Price,
                                                OrderNumber = _invoice.OrderNumber,
                                                Total = _invoice.Quantity * _products.Price,
                                                Note = _invoice.Note,
                                                CustomerId = _invoice.CustomerId,
                                                AccountId = _invoice.AccountId,
                                                CustomerName = _invoice.CustomerName,
                                                Email = _invoice.Email,
                                                Phone = _invoice.Phone,
                                                Address = _invoice.Address
                                            }
                                         )
                                        .Where(i => i.OrderId == id).ToList();
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId");
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TransactStatusId,Deleted,Paid,PaymentDate,PaymentId,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,TransactStatusId,Deleted,Paid,PaymentDate,PaymentId,Note")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'dbFrameContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
