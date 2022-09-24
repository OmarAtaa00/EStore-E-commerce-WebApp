using System;

namespace API.Dtos
{
    public class OrderDto
    {
        public string CartId { get; set; }
        public int DeliveryMethodID { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}

