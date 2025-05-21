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
        [Required]
        public string CustomerId { get; set; } 

        [Required]
        [MinLength(1)]
        public List<CreateOrderItemViewModel> Items { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        public int? AddressId { get; set; }
        
    }
}
