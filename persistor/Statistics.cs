using System;
using System.Data.SqlClient;
using Akka.Actor;
using btm.persistence.Calculator;

namespace btm.persistence
{
    public class Statistics : ReceiveActor
    {
        readonly string ConnectionString = "Server=.;Database=akka_transactions;Integrated Security=SSPI";

        public Statistics()
        {
            Receive<CustomerPaymentStatistics>(stat =>
            {
                Update(stat);
            });
        }

        private void Update(CustomerPaymentStatistics statistics)
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        cmd.CommandText = $@"
    MERGE INTO [dbo].[CustomerPaymentStatistics] WITH (HOLDLOCK) AS target
    USING (SELECT '{statistics.CustomerReference}', {statistics.DepositSuccessSum}, {statistics.DepositSuccessBaseSum}, {statistics.DepositSuccessCount}, {statistics.DepositTotalSum}, {statistics.DepositTotalBaseSum}, {statistics.DepositTotalCount}, 
				  {statistics.WithdrawalSuccessSum}, {statistics.WithdrawalSuccessBaseSum}, {statistics.WithdrawalSuccessCount}, {statistics.WithdrawalTotalSum}, {statistics.WithdrawalTotalBaseSum}, {statistics.WithdrawalTotalCount})
    AS source (customerReference, depositSuccessSum, depositSuccessBaseSum, depositSuccessCount, depositTotalSum, depositTotalBaseSum, depositTotalCount, 
			   withdrawalSuccessSum, withdrawalSuccessBaseSum, withdrawalSuccessCount, withdrawalTotalSum, withdrawalTotalBaseSum, withdrawalTotalCount)
    ON target.customerReference = source.customerReference
    WHEN MATCHED THEN
		   UPDATE
		   SET target.depositSuccessSum += {statistics.DepositSuccessSum},
			   target.depositSuccessBaseSum += {statistics.DepositSuccessBaseSum},
			   target.depositSuccessCount += {statistics.DepositSuccessCount},
			   target.depositTotalSum += {statistics.DepositTotalSum},
			   target.depositTotalBaseSum += {statistics.DepositTotalBaseSum},
			   target.depositTotalCount += {statistics.DepositTotalCount},

			   target.withdrawalSuccessSum += {statistics.WithdrawalSuccessSum},
			   target.withdrawalSuccessBaseSum += {statistics.WithdrawalSuccessBaseSum},
			   target.withdrawalSuccessCount += {statistics.WithdrawalSuccessCount},
			   target.withdrawalTotalSum += {statistics.WithdrawalTotalSum},
			   target.withdrawalTotalBaseSum += {statistics.WithdrawalTotalBaseSum},
			   target.withdrawalTotalCount += {statistics.WithdrawalTotalCount}
    WHEN NOT MATCHED THEN
          INSERT (customerReference, depositSuccessSum, depositSuccessBaseSum, depositSuccessCount, depositTotalSum, depositTotalBaseSum, depositTotalCount, 
			      withdrawalSuccessSum, withdrawalSuccessBaseSum, withdrawalSuccessCount, withdrawalTotalSum, withdrawalTotalBaseSum, withdrawalTotalCount)
          VALUES (source.customerReference, source.depositSuccessSum, source.depositSuccessBaseSum, source.depositSuccessCount, source.depositTotalSum, source.depositTotalBaseSum, source.depositTotalCount, 
			      source.withdrawalSuccessSum, source.withdrawalSuccessBaseSum, source.withdrawalSuccessCount, source.withdrawalTotalSum, source.withdrawalTotalBaseSum, source.withdrawalTotalCount);";

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\nStack trace: {ex.StackTrace}");
            }
        }
    }
}
