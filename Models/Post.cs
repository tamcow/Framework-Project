using System;
using System.Collections.Generic;

namespace IS220_PROJECT.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Contents { get; set; }
        public string? Thumb { get; set; }
        //public List<string>? img { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Author { get; set; }
        public string? Tags { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewfeed { get; set; }
        public int? Views { get; set; }

        public virtual Account? Account { get; set; }
    }
}
