using System;
using Newtonsoft.Json;


namespace ADP.SC.Modules.AlexaSkill.Web.Models
{
    public class ServiceOfferings
    {
        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }
       
    }
}