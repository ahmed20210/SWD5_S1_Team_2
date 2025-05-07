using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.mapper
{
    internal class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentViewModel>().ReverseMap();
           
        }
    
    }
}
