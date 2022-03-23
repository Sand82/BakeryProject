namespace Bakery.Models.Order
{
    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            items = new List<ItemFormViewModel>();
        }

        public int Id { get; set; }

        public bool IsPayed { get; set; }

        public string TotallPrice { get; set; }

        public int ItemsCount { get; set; }

        public IList<ItemFormViewModel> items { get; set; }

    }
}
