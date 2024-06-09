using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IS220_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace IS220_PROJECT.Controllers
{
    public class CartController : Controller
    {
        private readonly dbFrameContext _context;
        //private List<CustomerInvoice> _order;
        private readonly INotyfService _notyfService;
        public CartController(dbFrameContext context, INotyfService notyfService)
        {
            _context = context;
            //_order = order;
            _notyfService = notyfService;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var AccountId = HttpContext.Session.GetString("AccountId");
            if (AccountId != null)
            {
                var _order = _context.OrderDetails.Join(
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
                                                 Paid = _order.Paid
                                             })
                                        .Join(
                                            _context.Customers,
                                             _o => _o.CustomerId,
                                             _c => _c.CustomerId,
                                            (_o, _c) => new CustomerInvoice
                                            {
                                                OrderId = _o.OrderId,
                                                OrderDetailId = _o.OrdetailId,
                                                OrderDate = _o.OrderDate,
                                                ProductId = _o.ProductId,
                                                Quantity = _o.Quantity,
                                                OrderNumber = _o.OrderNumber,
                                                AccountId = _c.AccountId,
                                                CustomerId = _c.CustomerId,
                                                CustomerName = _c.FullName,
                                                Email = _c.Email,
                                                Phone = _c.Phone,
                                                Address = _c.Address,
                                                Paid = _o.Paid
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
                                                Address = _invoice.Address,
                                                Paid = _invoice.Paid
                                            }
                                         )
                                        .Where(i => i.AccountId == int.Parse(AccountId) && i.Paid == false).ToList();

                return View(_order);
            }
            return RedirectToAction("Login", "Account");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public async Task<IActionResult> Cart(CustomerInvoice customerInvoice)
        //{
        //    return View(customerInvoice);
        //}

        // GET: Cart/Details/5
        public async Task<IActionResult> AddToCart(int? ProductId, int amount, string? url)
        {
            if (ProductId == null)
            {
                return NotFound();
            }

            var product = _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == ProductId);
            if (product == null)
            {
                return NotFound();
            }

            int? OrderId;
            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId != null)
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AccountId == int.Parse(accountId));
                var neworder = _context.Orders.Where(o => o.CustomerId == customer.CustomerId).OrderBy(o => o.OrderDate).LastOrDefault();
                if (neworder.Paid == true)
                {
                    var order = new Order
                    {
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.Now,
                        Paid = false
                    };
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                }
                var _order = _context.Orders.Where(o => o.CustomerId == customer.CustomerId).OrderBy(o => o.OrderDate).LastOrDefault();
                OrderId = _order.OrderId;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var isExist = _context.OrderDetails.FirstOrDefault(p => p.ProductId == ProductId && p.OrderId == OrderId);
                if (isExist != null)
                {
                    isExist.Quantity += amount;
                    _context.Update(isExist);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = OrderId,
                        ProductId = ProductId,
                        Quantity = amount,
                        Discount = 0,
                        OrderNumber = 1,
                        Total = 0,
                        ShipDate = DateTime.Now.AddDays(3)
                    };
                    _context.Add(orderDetail);
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Đã thêm vào giỏ hàng");
                return Redirect((url == null) ? "/Index" : url);
            }
            catch (Exception e)
            {
                _notyfService.Error("Đã xảy ra lỗi! Vui lòng thử lại sau");
                return Redirect((url == null) ? "/Home/Index" : url);
            }

            return Redirect((url == null) ? "/Home/Index" : url);
        }


        // GET: Cart/Edit/5
        public async Task<IActionResult> UpdateCart(int? id, int amount)
        {
            var url = $"/Cart/Index";

            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            try
            {
                if (amount == 0)
                {
                    _context.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    orderDetail.Quantity = amount;
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Cập nhật thành công");
                //Debug.WriteLine(url);
                return Json(new { status = "success", redirectUrl = url });
            }
            catch (Exception)
            {

                _notyfService.Error("Đã xảy ra lỗi!");
                return Json(new { status = "error", redirectUrl = url });
                //return RedirectToAction("Index");
            }
            _notyfService.Error("Đã xảy ra lỗi!");
            return Json(new { status = "error", redirectUrl = url });
            //return RedirectToAction("Index");
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> DeleteAllItem(int? OrderId)
        {
            if (OrderId == null)
            {
                return NotFound();
            }

            var orderList = _context.OrderDetails
                .Where(m => m.OrderId == OrderId).ToList();
            if (orderList == null)
            {
                return NotFound();
            }
            try
            {
                foreach (var item in orderList)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Cập nhật giỏ hàng thành công");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _notyfService.Error("Đã có lỗi xảy ra");
                return RedirectToAction("Index");
                throw;
            }
            _notyfService.Error("Đã có lỗi xảy ra");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> isPaid()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");
            var order = _context.Orders.Where(o => o.Customer.AccountId == int.Parse(accountId)).ToList();
            List<CustomerInvoice> inv = new List<CustomerInvoice>();
            foreach(var item in order)
            {
                var orderD = _context.OrderDetails.Where(od => od.OrderId == item.OrderId).ToList();
                int total = 0;
                foreach(var itemD in orderD)
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductId == itemD.ProductId);
                    total += itemD.Quantity.Value*product.Price.Value;
                }
                var trans = _context.TransactStatuses.FirstOrDefault(t => t.TransactStatusId == item.TransactStatusId);
                CustomerInvoice curInv = new CustomerInvoice
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TransactStatus = trans.Status,
                    Total = total,
                    ShipDate = orderD[0].ShipDate
                };
                inv.Add(curInv);
            }
            return View(inv);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Payment(int? OrderId)
        {
            if (OrderId == null)
            {
                return NotFound();
            }

            var order = _context.Orders
                .FirstOrDefault(m => m.OrderId == OrderId);
            if (order == null)
            {
                return NotFound();
            }
            try
            {
                var orderD = _context.OrderDetails.Where(o => o.OrderId == OrderId).ToList();
                List<string> isInstock = new List<string>();
                foreach(var itemD in orderD)
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductId == itemD.ProductId);
                    product.UnitsInStock -= itemD.Quantity;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                order.Paid = true;
                order.TransactStatusId = 1;
                _context.Update(order);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thanh toán thành công");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _notyfService.Error("Đã có lỗi xảy ra");
                return RedirectToAction("Index");
                throw;
            }
            _notyfService.Error("Đã có lỗi xảy ra");
            return RedirectToAction("Index");
        }
    }
}
