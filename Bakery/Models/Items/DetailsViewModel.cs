﻿using Bakery.Models.Bakeries;

namespace Bakery.Models.Items
{
    public class DetailsViewModel
    {
        public string Name { get; set; }

        public string Price { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public ICollection<IngredientAddFormModel> Ingridients { get; set; }
    }
}