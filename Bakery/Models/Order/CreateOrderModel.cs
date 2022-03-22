namespace Bakery.Models.Order
{
    public class CreateOrderModel
    {
        public int Id { get; set; }

        public bool IsPay { get; set; }

        public IEnumerable<ItemFormViewModel> items { get; set; }

    }
}
