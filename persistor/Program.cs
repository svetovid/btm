using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.Query;
using Akka.Persistence.Query.Sql;
using Akka.Streams;
using Akka.Streams.Dsl;
using btm.persistence.Calculator;
using btm.shared.Messages;

namespace btm.persistence
{
    class Program
    {
        static IActorRef _statisticsActor;

        static void Main(string[] args)
        {
            var root = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText(Path.Combine(root, "Config.properties")));

            var actorsys = ActorSystem.Create("myactorsys", cfg);
            actorsys.ActorOf(Props.Create<Subscriber>(), "subscriber");
            actorsys.ActorOf(Props.Create<Destination>(), "destination");
            _statisticsActor = actorsys.ActorOf(Props.Create<Statistics>(), "statistics");

            Console.WriteLine("Press <ENTER> to enable streaming");
            Console.ReadLine();

            InitializePersisntenceQuery(actorsys);

            actorsys.WhenTerminated.Wait();
        }

        static void InitializePersisntenceQuery(ActorSystem actorSystem)
        {
            var readJournal = PersistenceQuery.Get(actorSystem)
                .ReadJournalFor<SqlReadJournal>("akka.persistence.query.journal.sql");

            // issue query to journal
            var source = readJournal.PersistenceIds()
                .Buffer(3, OverflowStrategy.Backpressure)                           // Backpressure
                .MergeMany(25, x =>                                                 // analogue of join in sql
                         readJournal.CurrentEventsByPersistenceId(x, 0, int.MaxValue));

            // materialize stream, consuming events
            var mat = ActorMaterializer.Create(actorSystem);
            source.RunForeach(envelope =>
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(envelope.Event));
                UpdateStatistics(envelope);
            }, mat);
        }

        static void UpdateStatistics(EventEnvelope envelope)
        {
            try
            {
                var message = (PaymentStatusChangedMessage)envelope.Event;
                var calculator = new CustomerPaymentCounterCalculator();
                var increment = calculator.GetPaymentChanges(message);

                var statistics = new CustomerPaymentStatistics
                {
                    CustomerReference = message.Customer.CustomerReference
                };

                if (message.Payment.MethodActionId == 1)
                {
                    statistics.DepositSuccessSum = increment.SuccessSum;
                    statistics.DepositSuccessBaseSum = increment.SuccessBaseSum;
                    statistics.DepositSuccessCount = increment.SuccessCount;
                    statistics.DepositTotalSum = increment.TotalSum;
                    statistics.DepositTotalBaseSum = increment.TotalBaseSum;
                    statistics.DepositTotalCount = increment.TotalCount;
                }
                else
                {
                    statistics.WithdrawalSuccessSum = increment.SuccessSum;
                    statistics.WithdrawalSuccessBaseSum = increment.SuccessBaseSum;
                    statistics.WithdrawalSuccessCount = increment.SuccessCount;
                    statistics.WithdrawalTotalSum = increment.TotalSum;
                    statistics.WithdrawalTotalBaseSum = increment.TotalBaseSum;
                    statistics.WithdrawalTotalCount = increment.TotalCount;
                }

                _statisticsActor.Tell(statistics);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\nStack trace: {ex.StackTrace}");
            }
        }
    }
}
