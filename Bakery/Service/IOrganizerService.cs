using Bakery.Areas.Task.Models;

namespace Bakery.Service
{
    public interface IOrganizerService
    {
        List<OrganizeViewModel> GetItems(DateTime date);

        string GetCustomProfit(DateTime fromDate, DateTime toDate);
    }
}
