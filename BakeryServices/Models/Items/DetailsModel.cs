﻿using System.ComponentModel.DataAnnotations;

namespace BakeryServices.Models.Items
{
    public class DetailsModel
    {
        public int productId { get; set; }

        [Range(1, 2000)]
        public int Quantity { get; set; }
    }
}
