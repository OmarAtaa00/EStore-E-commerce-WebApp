using System;
using Core.Entities;

namespace Core.OrderAggregate
{
    public class OrderItem : BaseEntity

    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
            Price = price;
            ItemOrdered = itemOrdered;
            Quantity = quantity;
        }

        public decimal Price { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }
        public int Quantity { get; set; }
    }
}
