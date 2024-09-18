using Newtonsoft.Json;
using System.Collections.Generic;

namespace Edent.Api.Models.PlayMobile
{
    public class SmsMessage
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }
}
