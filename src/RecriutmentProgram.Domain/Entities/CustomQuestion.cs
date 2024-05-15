using Newtonsoft.Json;

namespace RecriutmentProgram.Domain.Entities
{
    public class CustomQuestion : BaseEntity
    {

        [JsonProperty("programId")]
        public string ProgramId { get; set; }

        [JsonProperty("questionDescription")]
        public string? QuestionDescription { get; set; }

        [JsonProperty("fieldType")]
        public string FieldType { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("isInternal")]
        public bool IsInternal { get; set; }

        [JsonProperty("isDropdown")]
        public bool IsDropDown { get; set; }

        [JsonProperty("dropdownOptions")]
        public ICollection<Options> DropdownOptions { get; set; }

    }
}
