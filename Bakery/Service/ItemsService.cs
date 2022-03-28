using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.EditItem;
using Bakery.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Service
{
    public class ItemsService : IItemsService
    {
        private readonly BackeryDbContext data;
        private readonly IVoteService voteService;

        public ItemsService(BackeryDbContext data, IVoteService voteService)
        {
            this.data = data;
            this.voteService = voteService;            
        }

        public Item FindItem(string name, int quantity, decimal currPrice)
        {
            var item = this.data
                .Items
                .FirstOrDefault(i => i.ProductName == name && i.Quantity == quantity && i.ProductPrice == currPrice);

            return item;
        }

        public List<EditItemsFormModel> GetAllItems(int id)
        {
            var order = FindOrderById(id);

            var items = order.Items.Select(x => new EditItemsFormModel
            {
                Id = x.Id,
                Name = x.ProductName,
                Quantity = x.Quantity,
            })
            .ToList();

            return items;
        }

        public DetailsViewModel GetDetails(int id, string userId)
        {          
                      
            var averageVoteCount = (int)Math.Ceiling(voteService.GetAverage(id));           

            var product = this.data.
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
                .FirstOrDefault();           

            return product;
        }        

        public Order FindOrderById(int id) 
        {
            var order = this.data
                .Orders
                .Include(x => x.Items)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return order;
        }

        public Order FindOrderByUserId(string userId)
        {
            var order = this.data
                .Orders
                .Include(x => x.Items)
                .Where(x => x.UserId == userId && x.IsFinished == false)
                .FirstOrDefault();

            return order;
        }

        public Item FindItemById(int id)
        {
            var item = this.data
                .Items
                .Where(i => i.Id == id)
                .FirstOrDefault();

            return item;
        }

        public void DeleteItem(Item item, Order order)
        {
            order.Items.Remove(item);

            data.SaveChanges();
        }

        public void DeleteAllItems(Order order)
        {
            order.Items = null;

            data.SaveChanges();
        }

        public void ChangeItemQuantity(EditItemDataModel model)
        {

            var item = FindItemById(model.ItemId);

            item.Quantity = model.Quantity;

            this.data.SaveChanges();
        }
    }
}
