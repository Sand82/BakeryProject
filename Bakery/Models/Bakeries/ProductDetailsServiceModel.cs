namespace Bakery.Models.Bakeries
{
    public class ProductDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public ICollection<IngredientAddFormModel> Ingredients { get; set; }
    }
}
