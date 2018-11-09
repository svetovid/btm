using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using btm.paas.Messages;
using btm.shared.Messages;

namespace btm.paas.Actors
{
    public class PaasActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);

        private IActorRef _validationActor;
        private IActorRef _pspActor;

        private Dictionary<string, IActorRef> _payments = new Dictionary<string, IActorRef>();

        public PaasActor()
        {
            _validationActor = Context.ActorOf(Props.Create(() => new ValidationActor()), "validator");
            _pspActor = Context.ActorOf(Props.Create(() => new PspActor()), "psp");

            _log.Info("Paas up and running...");

            Receive<PaymentRequest>(msg => {
                //Log.Information("PaasActor: {0}, sender: {1}", Self.Path, Sender.Path);
                _log.Info("PaasActor: {0}, sender: {1}", Self.Path, Sender.Path);

                if (!_payments.ContainsKey(msg.Payment.PaymentReference))
                {
                    var paymentActor = Context.ActorOf(Props.Create(() =>
                        new PaymentActor(msg.Payment.PaymentReference, Sender,_validationActor, _pspActor)), msg.Payment.PaymentReference);
                    paymentActor.Tell(msg);

                    _payments.Add(msg.Payment.PaymentReference, paymentActor);
                }
                else
                {
                    _log.Info("Payment {0} is already being processed", msg.Payment.PaymentReference);
                }
            });

            Receive<TerminationResponse>(msg =>
            {
                _log.Info("Actor {0} can be disposed", msg.PaymentReference);

                Sender.GracefulStop(TimeSpan.FromSeconds(20)).ContinueWith((t) =>
                {
                    if(t.Result)
                        _payments.Remove(msg.PaymentReference);
                });
            });

            Receive<string>(msg =>
            {
                _log.Info("Message was received: {0}", msg);
            });
        }
    }
}