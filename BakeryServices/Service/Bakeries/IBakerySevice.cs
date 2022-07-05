using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Models.Items;

namespace Bakery.Service.Bakeries
{
    public interface IBakerySevice
    {
        Task<AllProductQueryModel> GetAllProducts(AllProductQueryModel query, string path);

        Product CreateProduct(BakeryFormModel formProduct);

        Task<ProductDetailsServiceModel> EditProduct(int id);

        Task<IEnumerable<BakryCategoryViewModel>> GetBakeryCategories();

        Task<Product> FindById(int id);

        Task<NamePriceDataModel> CreateNamePriceModel(int id);

        Task Edit(ProductDetailsServiceModel productModel, Product dataProdcuct);

        Task<bool> CheckCategory(int categoryId);

        Task Delete(Product product);

        Task AddProduct(Product product, string path);
    }
}
