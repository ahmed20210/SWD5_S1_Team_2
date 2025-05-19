using System.Collections.Generic;
using Business.ViewModels.PaymentViewModels;
using Business.ViewModels.CreateOrderViewModels;
using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;
using Business.ViewModels.AddressViewModels;

namespace Business.ViewModels.CheckoutViewModels
{
    public class CheckoutViewModel
    {
        public CreateOrderViewModel Order { get; set; }

        public CreatePaymentViewModel Payment { get; set; }

        public CreateAddressViewModel? ShippingAddress { get; set; }


    }
}
