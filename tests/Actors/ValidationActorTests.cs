using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using btm.paas.Actors;
using btm.paas.Messages;
using btm.shared.Messages;
using Xunit;

namespace btm.tests.Actors
{
    public class ValidationActorTests : TestKit
    {
        [Fact]
        public void ShouldReturnSuccess()
        {
            var paymentReference = "abc123";
            var paymentRequest = new PaymentRequest(
                "test.com",
                new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",2,2,DateTime.UtcNow,DateTime.UtcNow,paymentReference,string.Empty,0,"EUR",1),
                null, null);

            var actor = ActorOf<ValidationActor>();
            actor.Tell(paymentRequest);

            var received = ExpectMsg<ValidationResponse>();

            Assert.Equal(paymentReference, received.PaymentReference);
            Assert.Equal(0, received.StatusCode);
            Assert.Equal(string.Empty, received.StatusMessage);
        }

        [Fact]
        public void ShouldReturnErrorCodeOnMagicNumber()
        {
            var paymentReference = "abc123";
            var paymentRequest = new PaymentRequest(
                "test.com",
                new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",42,42,DateTime.UtcNow,DateTime.UtcNow,paymentReference,string.Empty,0,"EUR",1),
                null, null);

            var actor = ActorOf<ValidationActor>();
            actor.Tell(paymentRequest);

            var received = ExpectMsg<ValidationResponse>();

            Assert.Equal(paymentReference, received.PaymentReference);
            Assert.Equal(10005, received.StatusCode);
            Assert.Equal("Magic number", received.StatusMessage);
        }
    }
}