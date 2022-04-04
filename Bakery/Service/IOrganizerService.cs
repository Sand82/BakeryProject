using Bakery.Areas.Task.Models;

namespace Bakery.Service
{
    public interface IOrganizerService
    {
        List<OrganizeViewModel> GetItems(DateTime date);
    }
}
