using System.Collections.Generic;
using Akka.Actor;
using System;
using btm.shared.Messages;
using Akka.Cluster.Tools.PublishSubscribe;

namespace btm.persistence
{
    public class Destination : ReceiveActor
    {
        private Dictionary<string, IActorRef> _payments = new Dictionary<string, IActorRef>();

        public Destination()
        {
            try
            {
                var mediator = DistributedPubSub.Get(Context.System).Mediator;

                mediator.Tell(new Put(Self));

                Receive<string>(s =>
                {
                    Console.WriteLine($"Got {s}");

                    if (!_payments.ContainsKey(s))
                    {
                        var actor = Context.ActorOf(Props.Create(() => new Persistor(s)));
                        _payments.Add(s, actor);
                    }

                    _payments[s].Tell(s);
                });

                Receive<PaymentStatusChangedMessage>(s =>
                {
                    var paymentRef = s.Payment.PaymentReference;
                    if (!_payments.ContainsKey(paymentRef))
                    {
                        var actorRef = Context.ActorOf(Props.Create(() => new Persistor(paymentRef)), paymentRef);
                        _payments.Add(paymentRef, actorRef);
                    }

                    _payments[paymentRef].Tell(s);
                });
            }
            catch
            {
            }
        }
    }
}