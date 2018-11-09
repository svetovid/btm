using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using btm.shared.Messages;

namespace btm.web.Actors
{
    public static class PaasActorSystem
    {
        private static ActorSystem _system;
        private static IActorRef _bridgeActor;

        public static void ActivateActorSystem()
        {
            if (_system == null)
            {
                var cfg = ConfigurationFactory.ParseString(File.ReadAllText("Config.properties"));
                _system = ActorSystem.Create("paassystem", cfg);
            }
        }

        public static IActorRef GetSignalRBridgeActor(Action<string, PaymentStatus> send)
        {
            if (_bridgeActor == null)
            {
                if (_system == null)
                {
                    ActivateActorSystem();
                }

                _bridgeActor = _system.ActorOf(Props.Create(() =>
                new SignalRBridgeActor((connectionId, message) => send(connectionId, message))),
                "bridge");
            }

            return _bridgeActor;
        }
    }
}
