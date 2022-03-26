using Bakery.Data.Models;
using Bakery.Models.Items;

namespace Bakery.Service
{
    public interface IItemsService
    {
        DetailsViewModel GetDetails(int id, string userId);

        Item FindItem(string name, int quantity, decimal currPrice);

        IEnumerable<EditItemsFormModel> GetAllItems(int id);

        Order FindOrderById(int id);

        Order FindOrderByUserId(string userId);

        Item FindItemById(int id);

        void DeleteItem(Item item, Order order);

        void DeleteAllItems(Order order);        
    }
}
