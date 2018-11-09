using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace btm.paas.Messages
{
    internal class PspResponse
    {
        internal PspResponse(string payref, string status, string publicPaymentId)
        {
            PaymentReference = payref;
            Status = status;
            PublicPaymentId = publicPaymentId;
        }

        internal string PaymentReference { get; }

        internal string Status { get; }

        internal string PublicPaymentId { get; }
    }
}