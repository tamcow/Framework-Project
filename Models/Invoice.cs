namespace IS220_PROJECT.Models
{
    public class Invoice
    {
        public int InputId { get; set; }
        public int InputDetailId { get; set; }
        public DateTime? Date { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? InputPrice { get; set; }
        public string? Note { get; set; }
        //public Invoice(int inputId, DateTime? date, int? productId, int? quantity, int? inputPrice, string? note)
        //{
        //    InputId = inputId;
        //    Date = date;
        //    ProductId = productId;
        //    Quantity = quantity;
        //    InputPrice = inputPrice;
        //    Note = note;
        //}
    }
}
