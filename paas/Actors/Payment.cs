namespace btm.paas.Actors
{
    public class Payment
    {
        public string PaymentReference { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string CustomerReference { get; set; }

        public int MethodActionId { get; set; }

        public string PublicPaymentId { get; set; }

        public string ProviderAccountName { get; set; }

        public string Status { get; set; }
    }
}