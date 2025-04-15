

using System.ComponentModel.DataAnnotations;
using Domain;
using Domain.Entities;

namespace Domain.Entities;

    public class Coupon
    {

        public int Id { get; set; }

        [MaxLength(20)][Required]
        public string Code { get; set; }

        [Required]
        public CouponType Type { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public int UsageLimit { get; set; }

        public int UsageCount { get; set; } = 0;
        
        
        public int? MaxValue { get; set; }

        public int MinOrderValue { get; set; }


        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [ Required]
        public DateTime ExpiryDate { get; set; }

        public CouponStatus Status { get; set; }
        
        public int? BannerId { get; set; }
        public Banner? Banner { get; set; }

    }


