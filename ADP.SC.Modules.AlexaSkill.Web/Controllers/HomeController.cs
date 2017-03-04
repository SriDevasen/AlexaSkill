using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ADP.SC.Modules.AlexaSkill.Web.SpeechAssets;

namespace ADP.SC.Modules.AlexaSkill.Web.Controllers
{
     public class HomeController : ApiController
        {
            [Route("api/HR")]
            [HttpPost]
            public async Task<HttpResponseMessage> HR()
            {
                var speechlet = new HRSpeechlet();
                return await speechlet.GetResponseAsync(Request);
            }
        }
}