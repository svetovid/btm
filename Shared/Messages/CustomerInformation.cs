namespace btm.shared.Messages
{
    public class CustomerInformation
    {
        public CustomerInformation(string customerCountryCode, string customerReference)
        {
            CustomerCountryCode = customerCountryCode;
            CustomerReference = customerReference;
        }

        public string CustomerReference { get; }
        public string CustomerCountryCode { get; }
    }
}