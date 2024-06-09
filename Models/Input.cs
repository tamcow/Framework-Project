using System;
using System.Collections.Generic;

namespace IS220_PROJECT.Models
{
    public partial class Input
    {
        public Input()
        {
            InputDetails = new HashSet<InputDetail>();
        }

        public int InputId { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<InputDetail> InputDetails { get; set; }
    }
}
