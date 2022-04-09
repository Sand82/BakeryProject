using Bakery.Service;
using Bakery.Tests.Mock;
using Xunit;

namespace Bakery.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreatOrderShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var userId = "Some id";

            var result =  orderService.CreatOrder(userId);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatItemShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var userId = "Some id";

            var result = orderService.CreateItem(1, "Bread", 3.20m, 3, userId);

            Assert.NotNull(result);
        }
    }
}
