using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.PaymentViewModels
{
    public class CreatePaymentViewModel
    {
        public string TransactionId { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethods Method { get; set; }
    }
}