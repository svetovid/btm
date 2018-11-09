using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using System;
using Akka.Routing;
using btm.shared.Messages;

namespace btm.web.Actors
{
    public class SignalRBridgeActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private Dictionary<string, string> _connectionIdMap = new Dictionary<string, string>();

        public SignalRBridgeActor(Action<string, PaymentStatus> sendMessage)
        {
            try
            {
                var paasActor = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "paas");

                Receive<PaymentRequest>(msg =>
                {
                    _log.Info("Connection id: {0}", msg.ConnectionId);

                    if (_connectionIdMap.ContainsKey(msg.ConnectionId))
                    {
                        _connectionIdMap[msg.ConnectionId] = msg.Payment.PaymentReference;
                    }
                    else
                    {
                        _connectionIdMap.Add(msg.ConnectionId, msg.Payment.PaymentReference);
                    }

                    _log.Info("SignalRBridgeActor: {0}, sender: {1}", Self.Path, Sender.Path);

                    paasActor.Tell(msg);
                });

                Receive<PaymentStatus>(msg =>
                {
                    var connId = _connectionIdMap.SingleOrDefault(x => x.Value == msg.PaymentReference).Key;

                    _log.Info("Payment status connection id: {0}, sender: {1}", connId, Sender.Path);

                    sendMessage(connId, msg);

                    _log.Info("SignalRBridgeActor Status changed {0}: {1}", msg.PaymentReference, msg.Status);
                });

                Receive<string>(msg =>
                {
                    var connId = _connectionIdMap.FirstOrDefault().Key;

                    sendMessage(connId, new PaymentStatus("test", msg));
                });
            }
            catch
            {
            }
        }
    }
}