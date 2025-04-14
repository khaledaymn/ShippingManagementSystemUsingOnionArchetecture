using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Enums
{
    public enum OrderState
    {
        New,
        Pendding,
        DeliveredToTheRepresentative,
        Delivered,
        CannotBeReached,
        PostPoned,
        PartiallyDelivered,
        CanceledByCustomer,
        RejectedWithPayment,
        RejectedWithPartialPayment,
        RejectedWithoutPayment
    }
}
