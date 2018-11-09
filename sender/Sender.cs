using Akka.Actor;
using btm.shared.Messages;
using Akka.Cluster.Tools.PublishSubscribe;

namespace btm.sender
{
    public class Sender : ReceiveActor
    {
        public Sender()
        {
            try
            {
                var mediator = DistributedPubSub.Get(Context.System).Mediator;

                Receive<string>(str =>
                {
                    var upperCase = str.ToUpper();
                    mediator.Tell(new Send("/user/destination", str, true));
                });

                Receive<PaymentStatusChangedMessage>(str =>
                {
                    mediator.Tell(new SendToAll("/user/destination", str, true));
                });
            }
            catch
            {
            }
        }
    }
}