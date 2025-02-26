namespace Domain;

public enum PaymentStatus
{
    Pending,
    Failed,
    Refunded,
    Success,
    Canceled
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

