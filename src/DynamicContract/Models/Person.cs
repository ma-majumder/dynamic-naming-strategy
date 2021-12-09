using DynamicContract.Json.Converters;
using Newtonsoft.Json;
using System;

namespace DynamicContract.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonProperty("DateOfBirth")]
        [JsonConverter(typeof(IsoDateConverter))]
        public DateTime? DOB { get; set; }
    }
}
