using Bakery.Models.Home;

namespace Bakery.Service.Home
{
    public interface IHomeService
    {
        Task<CountViewModel> GetIndex();
    }
}
