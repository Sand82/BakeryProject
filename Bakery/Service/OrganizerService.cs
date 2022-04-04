using Bakery.Areas.Task.Models;
using Bakery.Data;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Service
{
    public class OrganizerService : IOrganizerService
    {
        private readonly BackeryDbContext data;

        public OrganizerService(BackeryDbContext data)
        {
            this.data = data;
        }

        public string GetCustomProfit(DateTime fromDate, DateTime toDate)
        {
            var models = this.data
                    .Items
                    .Include(o => o.Orders)
                    .Where(o => o.Orders.Any(o => o.DateOfDelivery >= fromDate &&
                    o.DateOfDelivery <= toDate &&
                    o.IsFinished == true))
                    .Select(i => new ProfitDataModel 
                    { 
                        Price = i.ProductPrice,
                        Quantity = i.Quantity,
                    })
                    .ToList();

            decimal totallProfit = 0.0M;

            foreach (var model in models)
            {
                totallProfit += model.Price * model.Quantity;
            }

            return totallProfit.ToString("f2") + '$';
        }

        public List<OrganizeViewModel> GetItems(DateTime date)
        {
            List<OrganizeViewModel> modelItems = new List<OrganizeViewModel>();

            Task.Run(() => 
            {
                for (int i = 1; i <= 5; i++)
                {

                    date = date.AddDays(1);

                    var items = this.data
                    .Items
                    .Include(o => o.Orders)
                    .Where(i => i.Orders.Any(o =>
                                             o.DateOfDelivery.Year == date.Year &&
                                             o.DateOfDelivery.Month == date.Month &&
                                             o.DateOfDelivery.Day == date.Day && o.IsFinished == true))
                    .ToList();

                    OrganizeViewModel model = new OrganizeViewModel();

                    model.DayOfOrder = date;

                    foreach (var item in items)
                    {
                        if (!model.Items.ContainsKey(item.ProductName))
                        {
                            model.Items.Add(item.ProductName, new Dictionary<decimal, int>());
                        }
                        if (!model.Items[item.ProductName].ContainsKey(item.ProductPrice))
                        {
                            model.Items[item.ProductName].Add(item.ProductPrice, 0);
                        }

                        model.Items[item.ProductName][item.ProductPrice] += item.Quantity;

                        model.TottalPrice += item.Quantity * item.ProductPrice;
                    }

                    model.ColapsValue = GetColapsValue(i);

                    modelItems.Add(model);
                }
            }).GetAwaiter().GetResult();
                       
                      
            return modelItems;
        }

        private string GetColapsValue(int number)
        {
            var value = string.Empty;

            var colaps = new Dictionary<int, string>()
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Tour" },
                { 5, "Five " },
            };

            value = colaps[number];

            return value;
        }
    }
}
