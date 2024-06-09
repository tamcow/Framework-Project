using System;
using System.Collections.Generic;

namespace IS220_PROJECT.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int? CatId { get; set; }
        public int? BrandId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Cpu { get; set; }
        public string? Cpudetail { get; set; }
        public string? Ram { get; set; }
        public string? TypeOfHardDrive { get; set; }
        public string? Capacity { get; set; }
        public string? TypeOfGpu { get; set; }
        public string? Gpu { get; set; }
        public string? Gpudetail { get; set; }
        public string? Os { get; set; }
        public string? MonitorDetail { get; set; }
        public double? Size { get; set; }
        public string? Weight { get; set; }
        public int? Price { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public string? Sku { get; set; }
        public int? UnitsInStock { get; set; }
        public int? SupplierId { get; set; }
        public string? Description { get; set; }
        public int? InputDetailId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Cat { get; set; }
        public virtual InputDetail? InputDetail { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
