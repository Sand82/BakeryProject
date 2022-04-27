using Bakery.Controllers;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.EditItem;
using Bakery.Service;
using Bakery.Service.Items;
using Bakery.Tests.Mock;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class EditItemControllerTests
    {
        [Fact]
        public void PostActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItems(data);

            data.Items.AddRange(items);

            data.SaveChanges();

            var model = new EditItemDataModel
            {
                ItemId = 1,
                Quantity = 3,
            };

            var itemsService = new ItemService(data, null);

            var controler = new EditItemController(itemsService);

            controler.Post(model);

            var result = data.Items.Where(i => i.Id == model.ItemId).FirstOrDefault();

            Assert.NotNull(result);

            Assert.Equal(model.Quantity, result.Quantity);
        }

        [Fact]
        public void PostActionShouldTrowExeptionWithIncorectProductId()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItems(data);

            data.Items.AddRange(items);

            data.SaveChanges();

            var model = new EditItemDataModel
            {
                ItemId = 10,
                Quantity = 3,
            };

            var itemsService = new ItemService(data, null);

            var controler = new EditItemController(itemsService);          

            var ex = Assert.Throws<System.NullReferenceException>(() => controler.Post(model));

            Assert.Equal("Not found", ex.Message);            
        }

        private List<Item> CreateListItems(BakeryDbContext data)
        {
            var items = new List<Item>()
            {
                new Item() { Id = 1, ProductId = 1, ProductName ="Bread", ProductPrice = 3.00m, Quantity = 3 },
                new Item() { Id = 2, ProductId = 2, ProductName ="Bread with broun fluor", ProductPrice = 3.50m, Quantity =4 },
                new Item() { Id = 3, ProductId = 3, ProductName ="Bread with somthing", ProductPrice = 4.00m, Quantity = 5 },
            };

            return items;
        }
    }
}
