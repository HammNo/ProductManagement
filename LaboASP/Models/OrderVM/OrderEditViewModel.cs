namespace ProductManagement.ASP.Models.OrderVM
{
    public class OrderEditViewModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public IEnumerable<OrderLineEditViewModel>? ExistingOrderLines { get; set; }
        public List<string>? UpdatedOrderLines { get; set; }
    }
}
