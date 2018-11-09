using System.Collections.Generic;
using System.Linq;

namespace btm.persistence.Calculator
{
    public static class PaymentStatusExtensions
    {
        /// <summary>
        ///     The payment final status is initiated by provider.
        /// </summary>
        private static readonly List<string> ProviderFinalStatuses;

        /// <summary>
        ///     The payment final status is initiated by user.
        /// </summary>
        private static readonly List<string> UserFinalStatuses = new List<string>
        {
            "Cancelled",
            "ChargedBack",
            "Refunded"
        };

        /// <summary>
        ///     The successful payment statuses.
        /// </summary>
        private static readonly List<string> SuccessfulStatuses;

        /// <summary>
        ///     The failed payment statuses.
        /// </summary>
        private static readonly List<string> FailedStatuses;

        /// <summary>
        ///     The payment final status (0x01AF).
        /// </summary>
        private static readonly List<string> FinalStatuses;

        static PaymentStatusExtensions()
        {
            SuccessfulStatuses = new List<string> { "Authorized", "Captured" };
            FailedStatuses = new List<string> { "Error", "Refused" };
            ProviderFinalStatuses = SuccessfulStatuses.Concat(FailedStatuses).Distinct().ToList();
            FinalStatuses = ProviderFinalStatuses.Concat(UserFinalStatuses).Distinct().ToList();
        }

        /// <summary>
        ///     Determines whether the specified payment status is final.
        /// </summary>
        /// <param name="status">The payment status.</param>
        public static bool IsFinal(this string status)
        {
            return FinalStatuses.Contains(status);
        }

        /// <summary>
        ///     Determines whether the specified payment status is successful.
        /// </summary>
        /// <param name="status">The payment status.</param>
        public static bool IsSuccessful(this string status)
        {
            return SuccessfulStatuses.Contains(status);
        }
    }
}
