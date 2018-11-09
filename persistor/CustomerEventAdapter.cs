using System.Collections.Immutable;
using Akka.Persistence.Journal;
using btm.shared.Messages;

namespace btm.persistence
{
    public class CustomerEventAdapter : IWriteEventAdapter
    {
        public string Manifest(object evt) => string.Empty;

        internal Tagged WithTag(object evt, string tag) => new Tagged(evt, ImmutableHashSet.Create(tag));

        public object ToJournal(object evt)
        {
            if (evt is PaymentStatusChangedMessage)
            {
                return WithTag(evt, ((PaymentStatusChangedMessage)evt).Customer.CustomerReference);
            }

            return evt;
        }
    }
}
