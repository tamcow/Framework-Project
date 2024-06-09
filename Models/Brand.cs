using System;
using System.Collections.Generic;

namespace IS220_PROJECT.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
