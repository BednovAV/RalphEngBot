using Newtonsoft.Json;

namespace Entities.Navigation
{
    public class CallbackQuerryItem
    {
        [JsonProperty("T")]
        public InlineMarkupType Type { get; set; }
        [JsonProperty("D")]
        public string Data { get; set; }
    }
}
