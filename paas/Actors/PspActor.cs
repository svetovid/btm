using Akka.Actor;
using Akka.Event;
using btm.paas.Messages;
using btm.shared.Messages;

namespace btm.paas.Actors
{
    public class PspActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);

        public PspActor()
        {
            Receive<PaymentRequest>(msg => {
                _log.Info("PspActor: {0}, sender: {1}", Self.Path, Sender.Path);

                //System.Threading.Tasks.Task.Delay(2000);
                System.Threading.Thread.Sleep(2000);

                var status = msg.Payment.PaymentAmount > 100 ? "Refused" : "Captured";
                Sender.Tell(new PspResponse(msg.Payment.PaymentReference, status, "qwertyy123"));
            });
        }
    }
}