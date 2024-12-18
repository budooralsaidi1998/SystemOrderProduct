﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace SystemProductOrder.models
{

    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderPorduct
    {
        public int OrderId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [ForeignKey("OrderId")]
        [JsonIgnore]
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }
}
