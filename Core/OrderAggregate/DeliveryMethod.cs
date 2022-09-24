using System;
using Core.Entities;

namespace Core.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }
        public string  Description  { get; set; }
        public decimal  Price { get; set; }
        public string DeliveryTime { get; set; }


    }
}
