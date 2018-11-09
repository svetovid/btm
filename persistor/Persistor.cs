using System;
using System.Collections.Generic;
using Akka;
using Akka.Persistence;
using btm.shared.Messages;

namespace btm.persistence
{
    public class Persistor : PersistentActor
    {
        private readonly string _referenceKey;
        private List<PaymentStatusChangedMessage> _events = new List<PaymentStatusChangedMessage>();

        public Persistor(string referenceKey)
        {
            _referenceKey = referenceKey;
        }

        public override string PersistenceId => _referenceKey;

        //protected override bool Receive(object message)
        //{
        //    return base.Receive(message);
        //}

        protected override bool ReceiveCommand(object message)
        {
            message.Match()
                .With<PaymentStatusChangedMessage>(msg =>
                {
                    //Console.WriteLine($"PaymentStatusChangedMessage Message has been received by Persistor {Self}");
                    Persist(msg, (m) =>
                    {
                        /*Update state*/
                        //Console.WriteLine(m);
                    });
                });

            return true;
        }

        protected override bool ReceiveRecover(object message)
        {
            //Console.WriteLine(_referenceKey);
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(message));
            //message.Match()
            //    .With<PaymentStatusChangedMessage>(msg => _events.Add(msg))
            //    .With<string>(msg => { })
            //    .With<SnapshotOffer>(offer =>
            //    {
            //        //Console.WriteLine("Event Recover Message has been received within snapshots");

            //        //_events = (List<Shared.Event>)offer.Snapshot;
            //        //Console.WriteLine($"Recovered state with {_events.Count} messages");
            //    });

            //Console.WriteLine("Recover Message has been received");

            return true;
        }
    }
}