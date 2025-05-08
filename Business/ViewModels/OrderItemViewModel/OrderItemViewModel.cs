using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.OrderItemViewModel
{
    public class OrderItemViewModel
    {
        public int OrderId { get; set; }  
        public int ProductId { get; set; }  

        public string ProductName { get; set; }  
        public int Quantity { get; set; }  
        public decimal TotalAmount { get; set; }  

        public int? DiscountId { get; set; }  
        public decimal DiscountAmount { get; set; } 
        
    }


}
