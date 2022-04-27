using Bakery.Areas.Task.Models;

namespace Bakery.Service.Organizers
{
    public interface IOrganizerService
    {
        List<OrganizeViewModel> GetItems(DateTime date);

        string GetCustomProfit(DateTime fromDate, DateTime toDate);
    }
}
