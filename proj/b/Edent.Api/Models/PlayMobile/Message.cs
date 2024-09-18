using Newtonsoft.Json;

namespace Edent.Api.Models.PlayMobile
{
    public class Message
    {
        [JsonProperty("message-id")]
        public string MessageId { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("sms")]
        public Sms Sms { get; set; }
    }
}
