namespace btm.shared
{
    public class Event
    {
        public Event(string referenceKey, string message, int messageType)
        {
            ReferenceKey = referenceKey;
            Message = message;
            MessageType = messageType;
        }

        public string ReferenceKey { get; }

        public string Message { get; }

        public int MessageType { get; }
    }
}
