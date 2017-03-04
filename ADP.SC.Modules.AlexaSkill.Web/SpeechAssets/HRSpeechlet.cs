using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using Newtonsoft.Json;
using ADP.SC.Modules.AlexaSkill.Web.Models;

namespace ADP.SC.Modules.AlexaSkill.Web.SpeechAssets
{
    public class HRSpeechlet : SpeechletAsync
    {
        ServiceOfferings _serviceofferings;
        ServiceTypes _servicetypes;
        private const string ServiceOfferingsIntent = "ServiceOfferingsIntent";
        private const string ServiceTypesIntent = "ServiceTypesIntent";
     
        public override Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            return Task.FromResult(GetWelcomeResponse());
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            var output = "Welcome to the ADP HR App. Do you need information on Service Offerings or Service Types.";
            return BuildSpeechletResponse("Welcome", output, false);
        }

        public async override Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)       
        {
            // Get intent from the request object.
            var intent = intentRequest.Intent;
            var intentName = intent?.Name;

            // Note: If the session is started with an intent, no welcome message will be rendered;
            // rather, the intent specific response will be returned.
            if (ServiceOfferingsIntent.Equals(intentName))
            {
                return await GetServiceOfferingsResponse(intent, session);
            }
            if (ServiceTypesIntent.Equals(intentName))
            {
                return await GetServiceOfferingsResponse(intent, session);
            }

            throw new SpeechletException("Invalid Intent");
        }

        public override Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        public override Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        private async Task<SpeechletResponse> GetServiceOfferingsResponse(Intent intent, Session session)
        {
            // Create response
            string output;          
            var endSession = false;
            // Retrieve and return the menu response
            _serviceofferings = await LoadServiceOfferings();
            output = _serviceofferings.ToString();
            // Return response, passing flag for whether to end the conversation
            return BuildSpeechletResponse(intent.Name, output, endSession);
        }
        private async Task<SpeechletResponse> GetServiceTypesResponse(Intent intent, Session session)
        {
            // Create response
            string output;
            var endSession = false;
            // Retrieve and return the menu response
            _servicetypes = await LoadServiceTypes();
            output = _servicetypes.ToString();
            // Return response, passing flag for whether to end the conversation
            return BuildSpeechletResponse(intent.Name, output, endSession);
        }
        
        private async Task<ServiceOfferings> LoadServiceOfferings()
        {
            var filePath = HttpContext.Current.Server.MapPath("/App_Data/ServiceOfferings.json");
            using (var reader = new StreamReader(filePath))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<ServiceOfferings>(json);
            }
        }

        private async Task<ServiceTypes> LoadServiceTypes()
        {
            var filePath = HttpContext.Current.Server.MapPath("/App_Data/ServiceTypes.json");
            using (var reader = new StreamReader(filePath))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<ServiceTypes>(json);
            }
        }

        /// <summary>
        /// Creates and returns the visual and spoken response with shouldEndSession flag
        /// </summary>
        /// <param name="title">Title for the companion application home card</param>
        /// <param name="output">Output content for speech and companion application home card</param>
        /// <param name="shouldEndSession">Should the session be closed</param>
        /// <returns>SpeechletResponse spoken and visual response for the given input</returns>
        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            // Create the Simple card content
            var card = new SimpleCard
            {
                Title = $"SessionSpeechlet - {title}",
                Content = $"SessionSpeechlet - {output}"
            };

            // Create the plain text output
            var speech = new PlainTextOutputSpeech { Text = output };

            // Create the speechlet response
            var response = new SpeechletResponse
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = speech,
                Card = card
            };
            return response;
        }
    }
}