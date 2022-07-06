using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Bakeries;
using BakeryServices.Models.EditItem;
using BakeryServices.Models.Items;
using BakeryServices.Service.Votes;

using Microsoft.EntityFrameworkCore;

namespace BakeryServices.Service.Items
{
    public class ItemService : IItemService
    {
        private readonly BakeryDbContext data;
        private readonly IVoteService voteService;

        public ItemService(BakeryDbContext data, IVoteService voteService)
        {
            this.data = data;
            this.voteService = voteService;
        }

        public async Task<Item> FindItem(string name, int quantity, decimal currPrice)
        {
            var item = await this.data
            .Items
            .FirstOrDefaultAsync(
            i => i.ProductName == name && i.Quantity == quantity && i.ProductPrice == currPrice);

            return item;
        }

        public async Task<List<EditItemsFormModel>> GetAllItems(int id)
        {
            var items = new List<EditItemsFormModel>();

            var order = await FindOrderById(id);

            if (order == null)
            {
                throw new System.NullReferenceException("Not found");
            }

            items = order.Items.Select(x => new EditItemsFormModel
            {
                Id = x.Id,
                Name = x.ProductName,
                Quantity = x.Quantity,
            })
           .ToList();

            return items;
        }

        public async Task<DetailsViewModel> GetDetails(int id, string userId)
        {
            var averageVoteCount = (int)Math.Ceiling(voteService.GetAverage(id));

            var product = await this.data.
            Products
           .Include(i => i.Ingredients)
           .Where(p => p.Id == id)
           .Select(p => new DetailsViewModel
           {
               Id = p.Id,
               Name = p.Name,
               Price = p.Price.ToString("f2"),
               Description = p.Description,
               ImageUrl = p.ImageUrl,
               Category = p.Category.Name,
               VoteCount = averageVoteCount,
               Vote = voteService.GetValue(userId, id),
               Quantity = 1,
               Ingridients = p.Ingredients.Select(i => new IngredientAddFormModel
               {
                   Name = i.Name,
               })
              .OrderBy(i => i.Name)
              .ToList()

           })
           .FirstOrDefaultAsync();

            return product;
        }

        public async Task<Order> FindOrderById(int id)
        {
            var order = await this.data
            .Orders
            .Include(x => x.Items)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            return order;
        }

        public async Task<Order> FindOrderByUserId(string userId)
        {
            var order = await this.data
           .Orders
           .Include(x => x.Items)
           .Where(x => x.UserId == userId && x.IsFinished == false)
           .FirstOrDefaultAsync();

            return order;
        }

        public async Task<Item> FindItemById(int id)
        {
            var item = await this.data
            .Items
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();

            return item;
        }

        public async Task DeleteItem(Item item, Order order)
        {
            order.Items.Remove(item);

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteAllItems(Order order)
        {
            order.Items = null;

            await this.data.SaveChangesAsync();
        }

        public async Task ChangeItemQuantity(EditItemDataModel model)
        {
            Item item = null;

            try
            {
                item = await FindItemById(model.ItemId);
            }
            catch (Exception)
            {
                throw new NullReferenceException();
            }

            item.Quantity = model.Quantity;

            await this.data.SaveChangesAsync();

        }
    }
}
