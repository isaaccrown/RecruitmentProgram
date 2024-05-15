using Newtonsoft.Json;

namespace RecriutmentProgram.Domain.Entities
{
    public class Options : BaseEntity
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("optionName")]
        public string OptionName { get; set; }
    }
}
