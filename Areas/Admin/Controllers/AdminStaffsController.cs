using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;

namespace IS220_PROJECT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("[controller]/[action]")]
    public class AdminStaffsController : Controller
    {
        private readonly dbFrameContext _context;
        public INotyfService _notyfService { get; }

        public AdminStaffsController(dbFrameContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminStaffs
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsStaff = _context.Staffs.AsNoTracking().Include(p => p.Account).Include(p => p.Account.Role).OrderByDescending(x => x.CreateDate);
            PagedList<Staff> models = new PagedList<Staff>(lsStaff, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            //var dbFrameContext = _context.Customers.Include(c => c.Account);
            return View(models);
        }

        // GET: Admin/AdminStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.Account).Include(p => p.Account.Role)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Admin/AdminStaffs/Create
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

        // POST: Admin/AdminStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,FullName,Birthday,Avatar,Address,Email,Phone,AccountId,CreateDate,Active")] Staff staff, [FromForm(Name = "fileAva")] IFormFile fileThumb)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    staff.FullName = Utils.Utils.ToTitleCase(staff.FullName);
                    if (fileThumb != null)
                    {
                        string extension = Path.GetExtension(fileThumb.FileName);
                        string img = Utils.Utils.formatVNString(staff.FullName) + extension;
                        staff.Avatar = await Utils.Utils.UploadFile(fileThumb, @"staffs", img.ToLower());
                    }
                    if (string.IsNullOrEmpty(staff.Avatar))
                        staff.Avatar = "default.png";
                    staff.CreateDate = DateTime.Now;
                    staff.ModifiedDate = DateTime.Now;
                    staff.Active = true;
                    _context.Add(staff);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Thêm nhân viên thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", staff.AccountId);
            return View(staff);
        }

        // GET: Admin/AdminStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.AsNoTracking().Include(p => p.Account).FirstOrDefaultAsync(p => p.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", staff.AccountId);
            return View(staff);
        }

        // POST: Admin/AdminStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,FullName,Birthday,Avatar,Address,Email,Phone,AccountId,CreateDate,Active")] Staff staff, [FromForm(Name = "fileAva")] IFormFile fileThumb)
        {
            if (id != staff.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        staff.FullName = Utils.Utils.ToTitleCase(staff.FullName);
                        if (fileThumb != null)
                        {
                            string extension = Path.GetExtension(fileThumb.FileName);
                            string img = Utils.Utils.formatVNString(staff.FullName) + extension;
                            staff.Avatar = await Utils.Utils.UploadFile(fileThumb, @"staffs", img.ToLower());
                        }
                        if (string.IsNullOrEmpty(staff.Avatar))
                            staff.Avatar = "default.png";
                        staff.ModifiedDate = DateTime.Now;
                        staff.Active = true;
                        _context.Update(staff);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Cập nhật nhân viên thành công");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StaffExists(staff.StaffId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", staff.AccountId);
            return View(staff);
        }

        // GET: Admin/AdminStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Admin/AdminStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'dbFrameContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
          return _context.Staffs.Any(e => e.StaffId == id);
        }
    }
}
