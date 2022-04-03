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

        public OrganizeViewModel GetItems(DateTime date,int days)
        {
           date = date.AddDays(days);

            var items = this.data
                .Items
                .Include(o => o.Orders)
                .Where(i => i.Orders.Any(o => 
                                         o.DateOfDelivery.Year == date.Year && 
                                         o.DateOfDelivery.Month == date.Month || 
                                         o.DateOfDelivery.Day == date.Day))
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


            return model;
        }
    }
}
