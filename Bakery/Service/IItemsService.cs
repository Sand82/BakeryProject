using Bakery.Data.Models;
using Bakery.Models.Items;

namespace Bakery.Service
{
    public interface IItemsService
    {
        DetailsViewModel GetDetails(int id, string userId);

        Item FindItem(string name, int quantity, decimal currPrice);
    }
}
