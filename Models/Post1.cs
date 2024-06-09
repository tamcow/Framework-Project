namespace IS220_PROJECT.Models
{
    public class Post1
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Contents { get; set; }
        public List<string>? Thumb { get; set; }
        //public List<string>? img { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Author { get; set; }
        public string? Tags { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewfeed { get; set; }
        public int? Views { get; set; }
    }
}
