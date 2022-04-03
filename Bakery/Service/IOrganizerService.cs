using Bakery.Areas.Task.Models;

namespace Bakery.Service
{
    public interface IOrganizerService
    {
        OrganizeViewModel GetItems(DateTime date, int days);
    }
}
