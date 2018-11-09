using System;

namespace btm.shared.Messages
{
    public class PaymentInformation
    {
        public PaymentInformation(string providerCurrencyCode,
            decimal providerAmount,
            string userCurrencyCode,
            decimal userAmount,
            string baseCurrencyCode,
            decimal baseFee,
            string paymentMethod,
            decimal baseAmount,
            decimal paymentAmount,
            DateTime created,
            DateTime changed,
            string paymentReference,
            string publicPaymentId,
            long paymentId,
            string currencyCode,
            int methodActionId)
        {
            ProviderAmount = providerAmount;
            UserCurrencyCode = userCurrencyCode;
            UserAmount = userAmount;
            BaseCurrencyCode = baseCurrencyCode;
            BaseFee = baseFee;
            PaymentMethod = paymentMethod;
            BaseAmount = baseAmount;
            PaymentAmount = paymentAmount;
            Created = created;
            Changed = changed;
            PaymentReference = paymentReference;
            PublicPaymentId = publicPaymentId;
            PaymentId = paymentId;
            CurrencyCode = currencyCode;
            MethodActionId = methodActionId;
            ProviderCurrencyCode = providerCurrencyCode;
        }

        public string ProviderCurrencyCode { get; }
        public decimal ProviderAmount { get; }
        public string UserCurrencyCode { get; }
        public decimal UserAmount { get; }
        public string BaseCurrencyCode { get; }
        public decimal BaseFee { get; }
        public string PaymentMethod { get; }
        public decimal BaseAmount { get; }
        public decimal PaymentAmount { get; }
        public DateTime Created { get; }
        public DateTime Changed { get; }
        public string PaymentReference { get; }
        public string PublicPaymentId { get; }
        public long PaymentId { get; }
        public string CurrencyCode { get; }
        public int MethodActionId { get; }
    }
}