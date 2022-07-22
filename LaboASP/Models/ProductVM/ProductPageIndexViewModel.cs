namespace ProductManagement.ASP.Models.ProductVM
{
    public class ProductPageIndexViewModel
    {
        public IEnumerable<ProductIndexViewModel> PageProducts { get; set; }
        public string SearchedName { get; set; }

        public int CurrentPage { get; set; }
        public int NbrPages { get; set; }
    }
}
