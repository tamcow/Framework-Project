using System;
using System.Collections.Generic;

namespace IS220_PROJECT.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public DateTime? ContractDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
