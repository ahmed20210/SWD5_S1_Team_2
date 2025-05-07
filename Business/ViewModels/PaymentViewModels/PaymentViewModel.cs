using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.PaymentViewModels
{
    public class PaymentViewModel
    {
        public string TransactionId { get; set; }

        public PaymentStatus Status { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public PaymentMethods Method { get; set; }
    }
}
