using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Avatar
{
    public class AvatarResponseRelicProperty
    {
        [JsonPropertyName("property_type")]
        public int PropertyType { get; set; }

        [JsonPropertyName("modify_property_type")]
        public int ModifyPropertyType { get; set; }
    }
}