namespace btm.paas.Messages
{
    internal class TerminationResponse
    {
        internal TerminationResponse(string paymentReference)
        {
            PaymentReference = paymentReference;
        }

        internal string PaymentReference { get; }
    }
}
