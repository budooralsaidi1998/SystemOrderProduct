﻿using System.ComponentModel.DataAnnotations;

namespace SystemProductOrder.DTO
{
    public class ProductInput
    {
        public string Name { get; set; }

        public string Description { get; set; }

       
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public decimal OverallRating { get; set; }
    }
}
