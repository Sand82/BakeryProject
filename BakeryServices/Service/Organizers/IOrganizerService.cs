using BakeryServices.Models.Organaizers;

namespace BakeryServices.Service.Organizers
{
    public interface IOrganizerService
    {
        Task<List<OrganizeViewModel>> GetItems(DateTime date);

        Task<string> GetCustomProfit(DateTime fromDate, DateTime toDate);
    }
}
