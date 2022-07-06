using BakeryServices.Models.Home;

namespace BakeryServices.Service.Home
{
    public interface IHomeService
    {
        Task<CountViewModel> GetIndex();
    }
}
