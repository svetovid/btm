using Akka.Actor;
using Akka.Event;
using btm.paas.Messages;
using btm.shared.Messages;

namespace btm.paas.Actors
{
    public class ValidationActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);

        public ValidationActor()
        {
            Receive<PaymentRequest>(msg => {
                _log.Info("ValidationActor: {0}, sender: {1}", Self.Path, Sender.Path);

                //System.Threading.Tasks.Task.Delay(2000);
                System.Threading.Thread.Sleep(2000);
                //FraudActor.Tell
                //IrActor.Tell
                //ClosedLoopActor.Tell
                //KycActor.Tell

                int code = 0;
                string statusMessage = "";

                if (msg.Payment.PaymentAmount == 42)
                {
                    code = 10005;
                    statusMessage = "Magic number";
                }

                Sender.Tell(new ValidationResponse(code, statusMessage, msg.Payment.PaymentReference));
            });
        }
    }
}