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
        private readonly BakeryDbContext data;
        private readonly IVoteService voteService;

        public ItemsService(BakeryDbContext data, IVoteService voteService)
        {
            this.data = data;
            this.voteService = voteService;
        }

        public Item FindItem(string name, int quantity, decimal currPrice)
        {
            var item = new Item();

            Task.Run(() =>
            {
                item = this.data
                .Items
                .FirstOrDefault(
                i => i.ProductName == name && i.Quantity == quantity && i.ProductPrice == currPrice);

            }).GetAwaiter().GetResult();

            return item;
        }

        public List<EditItemsFormModel> GetAllItems(int id)
        {
            var items = new List<EditItemsFormModel>();

            Task.Run(() =>
            {
                var order = FindOrderById(id);

                if(order == null)
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

            }).GetAwaiter().GetResult();

            return items;
        }

        public DetailsViewModel GetDetails(int id, string userId)
        {

            var averageVoteCount = (int)Math.Ceiling(voteService.GetAverage(id));

            DetailsViewModel? product = null;

            Task.Run(() =>
            {

                product = this.data.
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

            }).GetAwaiter().GetResult();

            return product;
        }

        public Order FindOrderById(int id)
        {
            Order? order = new Order();

            Task.Run(() =>
            {
                order = this.data
               .Orders
               .Include(x => x.Items)
               .Where(x => x.Id == id)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return order;
        }

        public Order FindOrderByUserId(string userId)
        {
            Order? order = new Order();

            Task.Run(() =>
            {
                order = this.data
               .Orders
               .Include(x => x.Items)
               .Where(x => x.UserId == userId && x.IsFinished == false)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return order;
        }

        public Item FindItemById(int id)
        {


            var item = new Item();

            Task.Run(() =>
            {
                item = this.data
               .Items
               .Where(i => i.Id == id)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return item;
        }

        public void DeleteItem(Item item, Order order)
        {
            Task.Run(() =>
            {
                order.Items.Remove(item);

                data.SaveChanges();

            }).GetAwaiter().GetResult();
        }

        public void DeleteAllItems(Order order)
        {
            Task.Run(() =>
            {
                order.Items = null;

                data.SaveChanges();

            }).GetAwaiter().GetResult();
        }

        public void ChangeItemQuantity(EditItemDataModel model)
        {
            Task.Run(() =>
            {
                var item = FindItemById(model.ItemId);

                item.Quantity = model.Quantity;

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }
    }
}
