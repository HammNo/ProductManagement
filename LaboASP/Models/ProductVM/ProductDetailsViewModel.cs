namespace ProductManagement.ASP.Models.ProductVM
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<string>? Categories { get; set; }

    }
}
