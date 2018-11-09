using Akka.Actor;
using System;
using Akka.Cluster.Tools.PublishSubscribe;

namespace btm.sender
{
    public class Publisher : ReceiveActor
    {
        public Publisher()
        {
            try
            {
                var mediator = DistributedPubSub.Get(Context.System).Mediator;

                Receive<string>(str =>
                {
                    mediator.Tell(new Publish("content", str));

                    Console.WriteLine("Message was sent");
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Message: {0}\nStack trace: {1}", ex.Message, ex.StackTrace);
            }
        }
    }
}