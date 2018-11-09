using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("btm.tests")]

namespace btm.paas.Messages
{
    internal class ValidationResponse
    {
        internal ValidationResponse(int code, string message, string paymentReference)
        {
            StatusCode = code;
            StatusMessage = message;
            PaymentReference = paymentReference;
        }

        internal int StatusCode { get; }

        internal string StatusMessage { get; }

        internal string PaymentReference { get; }
    }
}