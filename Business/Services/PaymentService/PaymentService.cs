using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.PaymentService
{
    public class PaymentService: IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PaymentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PaymentViewModel>> GetAllAsync()
        {
            var payments = await _context.Payments.ToListAsync(); 
            return payments.Select(p => new PaymentViewModel
            {
                TransactionId = p.TransactionId,
                Date = p.CreatedAt,
                Status = p.Status,
                Amount = p.Amount,
                Method = p.Method
            });
        }
    } }

