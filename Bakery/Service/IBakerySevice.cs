using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Models.Items;

namespace Bakery.Service
{
    public interface IBakerySevice
    {
        AllProductQueryModel GetAllProducts(AllProductQueryModel query);

        void CreateProduct(BakeryFormModel formProduct);

        ProductDetailsServiceModel EditProduct(int id);

        void Edit(int id, ProductDetailsServiceModel product);

        IEnumerable<BakryCategoryViewModel> GetBakeryCategories();

        Product FindById(int id);

        NamePriceDataModel CreateNamePriceModel(int id);

        void Delete(Product product);
    }
}
