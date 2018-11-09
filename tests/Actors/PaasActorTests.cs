using Akka.TestKit.Xunit2;
using btm.paas.Actors;
using Xunit;

namespace btm.tests.Actors
{
    public class PaasActorTests : TestKit
    {
        [Fact]
        public void ShouldLogUpAndRunnning()
        {
            EventFilter.Info("Paas up and running...")
                .ExpectOne(() => ActorOf<PaasActor>());
        }
    }
}