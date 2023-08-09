using System;
using System.Text.Json.Serialization;

namespace DynamicForms.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InputType
	{
		Text,
		Integer,
		Double,
		Float,
		DateTime,
		Radio,
		Checkbox,
		Option,
	}
}

