using System;
using Newtonsoft.Json;

namespace Hospital.BaseClasses.Models
{
    public class CityCount
    {
        [JsonProperty]
        public string CityName { get; set; } 
        [JsonProperty]
        public int Count { get; set; } 
    }
}
