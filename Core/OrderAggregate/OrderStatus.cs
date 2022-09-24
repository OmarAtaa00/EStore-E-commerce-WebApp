using System;
using System.Runtime.Serialization;

namespace Core.OrderAggregate
{


    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Received")]
        PaymentReceived,
        [EnumMember(Value = "Failed")]
        PaymentFailed,
    }

}
