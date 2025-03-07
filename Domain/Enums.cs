namespace Domain;

public enum PaymentStatus
{
    Pending,
    Failed,
    Refunded,
    Success,
    Canceled
}
public enum BoosterTypes
{
    Discount,
    Shipping,
    Reward,
    Coupon
}

public enum PaymentMethods
{
    CashOnDelivery,
    Stripe,
    PayPal,
    ApplePay,
}

public enum RefundStatus
{
    Pending,
    Processing,
    Completed,
    Canceled,
    Rejected
}
public enum OrderStatus  
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public enum LogStatus
{
    Success,
    Failed,
    Pending
}
public enum LogMethods
{
    GET,
    POST,
    PUT,
    DELETE,
    PATCH
}
public enum CouponType
{
    Percentage,   
    FixedAmount
}
public enum CouponStatus
{
    Active,   
    Expired,  
    Inactive  
}

public enum ProductStatus
{
    Available,
    OutOfStock,
    Discontinued
}
public enum UserRole
{
    Admin,
    Client
}

