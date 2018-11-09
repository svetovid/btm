namespace btm.shared.Messages
{
    public class PaymentRequest
    {
        public PaymentRequest(string siteName,
            PaymentInformation payment,
            CustomerInformation customer,
            string connectionId)
        {
            SiteName = siteName;
            Payment = payment;
            Customer = customer;
            ConnectionId = connectionId;
        }

        public string SiteName { get; }

        public PaymentInformation Payment { get; }
        public CustomerInformation Customer { get; }
        public string ConnectionId { get; }
    }
}