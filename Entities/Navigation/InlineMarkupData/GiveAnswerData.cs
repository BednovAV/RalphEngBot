using Newtonsoft.Json;

namespace Entities.Navigation.InlineMarkupData
{
    public class GiveAnswerData
    {
        [JsonProperty("QId")]
        public int QuestionId { get; set; }
        [JsonProperty("V")]
        public string Value { get; set; }
    }
}
