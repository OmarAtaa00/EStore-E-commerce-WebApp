using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(string buyerEmail, Address shipToAddress,
         DeliveryMethod deliveryMethod,
          IReadOnlyList<OrderItem> orderItems,
           decimal subTotal) ///int paymentId)
        {
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            // PaymentId = paymentId;
            BuyerEmail = buyerEmail;
        }
        public string BuyerEmail { get; set; }

        //Sqlite don't like DateTimeOffset type
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        // public int PaymentId { get; set; } //Later when implementing payment 

        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }
    }
}



//Method to get the total  public decimal GetTotal (){return SubTotal+DeliveryMethod.Price}