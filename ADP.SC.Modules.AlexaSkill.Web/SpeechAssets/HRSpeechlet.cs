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
    public class HRSpeechlet //: SpeechletAsync
    {        
        private const string ServiceOfferingsIntent = "ServiceOfferingsIntent";
        private const string ServiceTypesIntent = "ServiceTypesIntent";
     
        public Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            return Task.FromResult(GetWelcomeResponse());
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            var output = "Welcome to the ADP HR App. Do you need information on Service Offerings or Service Types.";
            return BuildSpeechletResponse("Welcome", output, false);
        }

        public async Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)       
        {
            // Get intent from the request object.
            var intent = intentRequest.Intent;
            var intentName = intent?.Name;

            // Note: If the session is started with an intent, no welcome message will be rendered;
            // rather, the intent specific response will be returned.
            if (ServiceOfferingsIntent.Equals(intentName) || ServiceTypesIntent.Equals(intentName))
            {
                return await GetIntentResponse(intent, session);
            }           

            throw new SpeechletException("Invalid Intent");
        }

        public Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        public Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        private async Task<SpeechletResponse> GetIntentResponse(Intent intent, Session session)
        {
            // Create response
            const bool endSession = false;
            // Retrieve and return the menu response
            var response = await LoadIntentData(intent.Name);
            var output = response;
            // Return response, passing flag for whether to end the conversation
            return BuildSpeechletResponse(intent.Name, output, endSession);
        }

        private async Task<string> LoadIntentData(string intentName)
        {
            var filePath = HttpContext.Current.Server.MapPath($"/App_Data/{intentName}.json");
            using (var reader = new StreamReader(filePath))
            {
                var json = await reader.ReadToEndAsync();
                return json;
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