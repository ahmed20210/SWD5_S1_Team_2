namespace Business.ViewModels.CheckoutViewModels
{
    public class CheckoutResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public int? OrderId { get; private set; }

        private CheckoutResult(bool isSuccess, string message, int? orderId = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            OrderId = orderId;
        }

        public static CheckoutResult Success(int orderId)
        {
            return new CheckoutResult(true, "Checkout successful", orderId);
        }

        public static CheckoutResult Failed(string message)
        {
            return new CheckoutResult(false, message);
        }
    }
}
