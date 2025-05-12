using Business.ViewModels.OrderViewModels;
using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.CreateOrderViewModels
{
    public class CreateOrderViewModel
    {
        public string CustomerId { get; set; }

        [Required]
        public List<CreateOrderItemViewModel> Items { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public int TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
