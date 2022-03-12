using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;

namespace Bakery.Service
{
    public interface IBakerySevice
    {
        AllProductQueryModel GetAllProducts(AllProductQueryModel query);

        void CreateProduct(BakeryAddFormModel formProduct);         
    }
}
