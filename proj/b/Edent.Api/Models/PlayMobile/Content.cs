using Newtonsoft.Json;

namespace Edent.Api.Models.PlayMobile
{
    public class Content
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
