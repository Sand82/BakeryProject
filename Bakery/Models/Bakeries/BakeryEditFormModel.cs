﻿namespace Bakery.Models.Bakeries
{
    public class BakeryEditFormModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }

        public int AuthorId { get; set; }

        public ICollection<IngredientAddFormModel> Ingredients { get; set; }
    }
}
