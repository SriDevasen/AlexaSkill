using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ADP.SC.Modules.AlexaSkill.Web.SpeechAssets;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.Speechlet;

namespace ADP.SC.Modules.AlexaSkill.Web.Controllers
{
  public class HomeController : ApiController
  {
    [Route("api/HR")]
    [HttpPost]
    public async Task<HttpResponseMessage> HR()
    {
      var speechlet = new HRSpeechlet();

      var intent = new Intent() { Name = "ServiceOfferingsIntent" };
      var intentRequest = new IntentRequest("Apples", DateTime.Now, intent);
      
      var yx = await speechlet.OnIntentAsync(intentRequest, new Session());
      
      var x = new SpeechletResponseEnvelope()
      {
        Version = "1",
        Response = yx,
        SessionAttributes = new Dictionary<string, string>()
      }.ToJson();

      var httpResponseMessage2 = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StringContent(x, Encoding.UTF8, "application/json")
      };

      return httpResponseMessage2;
    }
  }
}