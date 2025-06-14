﻿using DomainLayer.Models.ProductModule;

namespace DomainLayer.Models.OrderModule
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductItemOrdered Product { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
