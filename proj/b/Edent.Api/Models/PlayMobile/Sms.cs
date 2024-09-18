using Newtonsoft.Json;

namespace Edent.Api.Models.PlayMobile
{
    public class Sms
    {
        [JsonProperty("originator")]
        public string Originator { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }
    }
}
