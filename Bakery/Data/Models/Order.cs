﻿using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.Items = new HashSet<Item>();  
        }

        [Key]
        public int Id { get; init; }

        public string UserId { get; set; }

        public DateTime DateOfOrder { get; set; } = DateTime.UtcNow;

        public DateTime DateOfDelivery { get; set; }               

        public bool IsPayed { get; set; } = false;

        //public decimal TottalPrice { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
