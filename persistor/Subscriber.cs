using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace btm.persistence
{
    public class Subscriber : ReceiveActor
    {
        public Subscriber()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            mediator.Tell(new Subscribe("content", Self));

            Receive<string>(s =>
            {
                Console.WriteLine($"Got {s}");
            });

            Receive<SubscribeAck>(subscribeAck =>
            {
                if (subscribeAck.Subscribe.Topic.Equals("content")
                    && subscribeAck.Subscribe.Ref.Equals(Self)
                    && subscribeAck.Subscribe.Group == null)
                {
                    Console.WriteLine("subscribing");
                }
            });
        }
    }
}