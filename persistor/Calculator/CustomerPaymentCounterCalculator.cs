using btm.shared.Messages;

namespace btm.persistence.Calculator
{
    public class CustomerPaymentCounterCalculator
    {
        public virtual PaymentStatisticIncrement GetPaymentChanges(PaymentStatusChangedMessage paymentChangeMsg)
        {
            var increment = new PaymentStatisticIncrement();

            //skip message processing if current status is intermediate
            if (!IsCurrentStatusFinal(paymentChangeMsg))
            {
                return increment;
            }

            // Update total count
            PopulatePaymentTotalCounters(increment, paymentChangeMsg);

            var currentStatus = paymentChangeMsg.Status;

            // Here we work with payments in final status ONLY!
            PopulatePaymentSuccessCounters(increment, paymentChangeMsg, currentStatus);

            return increment;
        }

        private static void PopulatePaymentTotalCounters(PaymentStatisticIncrement increment, PaymentStatusChangedMessage paymentChangeMsg)
        {
            increment.TotalCount++;
            increment.TotalSum = paymentChangeMsg.Payment.PaymentAmount;
            increment.TotalBaseSum = paymentChangeMsg.Payment.BaseAmount;
        }

        private static void PopulatePaymentSuccessCounters(PaymentStatisticIncrement increment,
            PaymentStatusChangedMessage paymentChangeMsg, string currentStatus)
        {
            var sign = CalculatePaymentStatusIncrementSign(currentStatus);

            increment.SuccessCount += sign;
            increment.SuccessSum += sign * paymentChangeMsg.Payment.PaymentAmount;
            increment.SuccessBaseSum += sign * paymentChangeMsg.Payment.BaseAmount;
        }

        private static int CalculatePaymentStatusIncrementSign(string currentStatus)
        {
            if (currentStatus.IsSuccessful())
            {
                return 1; // OK => increase counter
            }
            return 0; // NOK => do not affect counter
        }

        private static bool IsCurrentStatusFinal(PaymentStatusChangedMessage message)
        {
            var currentStatus = message.Status;
            return currentStatus.IsFinal();
        }
    }
}
