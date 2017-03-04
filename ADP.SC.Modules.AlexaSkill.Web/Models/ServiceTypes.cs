using System;
using Newtonsoft.Json;


namespace ADP.SC.Modules.AlexaSkill.Web.Models
{
    public class ServiceTypes
    {
        [JsonProperty("BusinessType")]
        public string BusinessType { get; set; }

    }
}