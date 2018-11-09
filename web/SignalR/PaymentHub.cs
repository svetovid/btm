using System;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR;
using btm.shared.Messages;
using Serilog;
using btm.web.Actors;

namespace btm.web.SignalR
{
    public class PaymentHub : Hub
    {
        private IActorRef _bridgeActor;
        private IHubContext<PaymentHub> _hubContext;

        public PaymentHub(IHubContext<PaymentHub> hubContext)
        {
            _bridgeActor = PaasActorSystem.GetSignalRBridgeActor(Send);
            _hubContext = hubContext;
        }

        public void Deposit(int amount)
        {
            Log.Information("Make {0} EUR Deposit", amount);

            var request = new PaymentRequest("test.com",
                new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",amount,amount,DateTime.UtcNow,DateTime.UtcNow,Guid.NewGuid().ToString(),string.Empty,0,"EUR",1),
                new CustomerInformation("SE",Guid.NewGuid().ToString()),
                Context.ConnectionId);

            _bridgeActor.Tell(request);
        }

        private void Send(string connectionId, PaymentStatus message)
        {
            _hubContext.Clients.Client(connectionId).SendAsync("UpdateStatus", message);
        }
    }
}