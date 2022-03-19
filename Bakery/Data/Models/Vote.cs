﻿namespace Bakery.Data.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int MyProperty { get; set; }

        public Product Product { get; set; }

        public string UsreId { get; set; }

        public byte Value { get; set; }
    }
}
