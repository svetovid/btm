namespace btm.persistence.Calculator
{
    public class CustomerPaymentStatistics
    {
        public string CustomerReference { get; set; }

        public decimal DepositSuccessSum { get; set; }
        public decimal DepositSuccessBaseSum { get; set; }
        public int DepositSuccessCount { get; set; }
        public decimal DepositTotalSum { get; set; }
        public decimal DepositTotalBaseSum { get; set; }
        public int DepositTotalCount { get; set; }

        public decimal WithdrawalSuccessSum { get; set; }
        public decimal WithdrawalSuccessBaseSum { get; set; }
        public int WithdrawalSuccessCount { get; set; }
        public decimal WithdrawalTotalSum { get; set; }
        public decimal WithdrawalTotalBaseSum { get; set; }
        public int WithdrawalTotalCount { get; set; }
    }
}
