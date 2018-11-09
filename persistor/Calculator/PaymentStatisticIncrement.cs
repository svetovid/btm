namespace btm.persistence.Calculator
{
    public class PaymentStatisticIncrement
    {
        /// <summary>
        /// Gets or sets aggregated payments count, processed in this aggregation.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets aggregated payments sum in the payment's currency.
        /// </summary>
        public decimal TotalSum { get; set; }

        /// <summary>
        ///  Gets or sets aggregated payments sum in base currency.
        /// </summary>
        public decimal TotalBaseSum { get; set; }

        /// <summary>
        /// Gets or sets count of successfully finished payments.
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Gets or sets sum of successfully finished payments in the payment`s currency.
        /// </summary>
        public decimal SuccessSum { get; set; }

        /// <summary>
        /// Gets or sets sum of successfully finished payments in base currency.
        /// </summary>
        public decimal SuccessBaseSum { get; set; }
    }
}
