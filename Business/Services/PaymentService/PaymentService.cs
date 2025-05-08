using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.PaymentService
{
    public class PaymentService : IPaymentService
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


        public async Task CreateAsync(CreatePaymentViewModel model)
        {
            var payment = new Payment
            {
                TransactionId = model.TransactionId,
                OrderId = model.OrderId,
                Amount = model.Amount,
                Status = model.Status,
                Method = model.Method,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<UpdateViewModel> GetByIdForUpdateAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new UpdateViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }

        public async Task<PaymentViewModel> GetByIdAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new PaymentViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                Date = payment.CreatedAt,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }




        public async Task<PaymentViewModel> EditAsync(int id, UpdateViewModel model)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return null;
            }

            payment.Amount = model.Amount;
            payment.Status = model.Status;
            payment.Method = model.Method;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return new PaymentViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                OrderId = payment.OrderId,
                Date = payment.CreatedAt,
                Amount = payment.Amount,
                Status = payment.Status,
                Method = payment.Method
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

