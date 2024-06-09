using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace IS220_PROJECT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("[controller]/[action]")]
    public class AdminCustomersController : Controller
    {
        private readonly dbFrameContext _context;
        public INotyfService _notyfService { get; }

        public AdminCustomersController(dbFrameContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCustomers
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCustomers = _context.Customers.AsNoTracking().OrderByDescending(x => x.CreateDate);
            PagedList<Customer> models = new PagedList<Customer>(lsCustomers, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            //var dbFrameContext = _context.Customers.Include(c => c.Account);
            return View(models);
        }

        // GET: Admin/AdminCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.AsNoTracking().Include(p => p.Account).FirstOrDefaultAsync(p => p.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create(bool newAccount = false)
        {
            var lsaccount = _context.Accounts.AsNoTracking().ToList();
            int accountId = 0;
            if (newAccount == true)
                accountId = lsaccount[lsaccount.Count - 1].AccountId;
            ViewBag.AccountId = accountId; 
            ViewData["Roles"] = new SelectList(_context.Roles, "RoleId", "RoleName", 2);
            return View();
        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Birthday,Avatar,Address,Email,Phone,CreateDate,Active,AccountId")] Customer customer, [FromForm(Name = "fileAva")] IFormFile fileThumb)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer.FullName = Utils.Utils.ToTitleCase(customer.FullName);
                    if (fileThumb != null)
                    {
                        string extension = Path.GetExtension(fileThumb.FileName);
                        string img = Utils.Utils.formatVNString(customer.FullName) + extension;
                        customer.Avatar = await Utils.Utils.UploadFile(fileThumb, @"customers", img.ToLower());
                    }
                    if (string.IsNullOrEmpty(customer.Avatar))
                        customer.Avatar = "default.png";
                    customer.CreateDate = DateTime.Now;
                    customer.ModifiedDate = DateTime.Now;
                    customer.Active = true;
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Thêm khách hàng thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.AsNoTracking().Include(p => p.Account).FirstOrDefaultAsync(p => p.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // POST: Admin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Birthday,Avatar,Address,Email,Phone,CreateDate,Active,AccountId")] Customer customer, [FromForm(Name = "fileAva")] IFormFile fileThumb)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }
            var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();
            if (ModelState.IsValid)
            {
                try
                {
                    customer.FullName = Utils.Utils.ToTitleCase(customer.FullName);
                    if (fileThumb != null)
                    {
                        string extension = Path.GetExtension(fileThumb.FileName);
                        string img = Utils.Utils.formatVNString(customer.FullName) + extension;
                        customer.Avatar = await Utils.Utils.UploadFile(fileThumb, @"customers", img.ToLower());
                    }
                    if (string.IsNullOrEmpty(customer.Avatar))
                        customer.Avatar = "default.png";
                    customer.ModifiedDate = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'dbFrameContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
