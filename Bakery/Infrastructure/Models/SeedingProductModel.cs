namespace Bakery.Infrastructure
{
    internal class SeedingProductModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public List<SeedingIngredientModel> Ingredients { get; set; }
    }
}