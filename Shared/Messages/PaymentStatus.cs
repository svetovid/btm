namespace btm.shared.Messages
{
    public class PaymentStatus
    {
        public PaymentStatus(string paymentReference, string status)
        {
            PaymentReference = paymentReference;
            Status = status;
        }

        public string PaymentReference { get; }

        public string Status { get; }
    }
}