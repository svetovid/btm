using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using btm.shared.Messages;

namespace btm.sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
            var cfg = ConfigurationFactory.ParseString(File.ReadAllText(Path.Combine(root, "Config.properties")));

            Console.WriteLine("To Send - enter 1");
            Console.WriteLine("To Publish - enter text");

            var actorSystem = ActorSystem.Create("myactorsys", cfg);
            var publisher = actorSystem.ActorOf(Props.Create<Publisher>(), "publisher");
            var sender = actorSystem.ActorOf(Props.Create<Sender>(), "sender");

            while (true)
            {
                var opt = Console.ReadLine();
                if (opt == "exit")
                {
                    break;
                }
                else if (opt == "1")
                {
                    GenerateMessages(sender);
                }
                else
                {
                    publisher.Tell(opt);
                }
            }

            actorSystem.WhenTerminated.Wait();
        }

        static void GenerateMessages(IActorRef actor)
        {
            var randomizer = new Random();
            var paymentId = 1;

            var listOfCustomers =
                new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                };

            System.Threading.Tasks.Parallel.ForEach(listOfCustomers, cust =>
            {
                var listOfPaymentRefs =
                    new List<string>
                    {
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString()
                    };

                foreach (var pr in listOfPaymentRefs)
                {
                    var methodAction = randomizer.Next(1, 3);
                    var createdDate = DateTime.Now;
                    var randAmount = randomizer.Next(100);
                    var currencyCode = "EUR";
                    var fee = randAmount * 0.05m;

                    var randomStatuses = new List<string> { "Authorized", "Captured", "Error", "Refused" };
                    var statusIndex = randomizer.Next(4);
                    var statuses = new List<string> { "Created", "InProcess", randomStatuses[statusIndex] };

                    foreach (var st in statuses)
                    {
                        System.Threading.Thread.Sleep(randomizer.Next(100, 500));

                        var paymentMessage =
                            new PaymentStatusChangedMessage("test.com",
                            new PaymentInformation(currencyCode, randAmount, currencyCode, randAmount, currencyCode, fee, "VISA", randAmount, randAmount, createdDate, DateTime.Now, pr, pr, paymentId, currencyCode, methodAction),
                            new CustomerInformation("GB", cust), st);

                        actor.Tell(paymentMessage);

                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(paymentMessage));
                    }
                }

                paymentId++;
            });
        }
    }
}
