namespace btm.shared.Messages
{
    public class PaymentStatusChangedMessage
    {
        public PaymentStatusChangedMessage(string siteName,
            PaymentInformation payment,
            CustomerInformation customer, string status)
        {
            SiteName = siteName;
            Payment = payment;
            Customer = customer;
            Status = status;
        }

        public string SiteName { get; }

        public PaymentInformation Payment { get; }

        public CustomerInformation Customer { get; }

        public string Status { get; }
    }
}