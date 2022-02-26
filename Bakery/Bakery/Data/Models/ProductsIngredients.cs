namespace Bakery.Data.Models
{
    public class ProductsIngredients
    {
        public int BakedDishId { get; set; }

        public Product BakedDish { get; set; }


        public int ProductId { get; set; }

        public Ingredient Product { get; set; }
    }
}
