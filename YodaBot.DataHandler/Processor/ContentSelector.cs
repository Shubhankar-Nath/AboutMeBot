using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using YodaBot.DataHandler.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder;
using AdaptiveCards;
using YodaBot.Utility.Logging;

namespace YodaBot.DataHandler.Processor
{
    public static class ContentSelector
    {
        public static Activity getAnswer(Activity customResponse,string responseJson, IBotTelemetryClient telemetryClient) 
        {
            Answers answersHolder = null;
            try
            {
                answersHolder = JsonConvert.DeserializeObject<Answers>(responseJson.Trim());
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e, TelemetryMetaData.BindExceptionEvent(TelemetryKey.JsonDeserializationError,"QnA Answer Json to Answer Model"));
            }

            switch (answersHolder.Type)
            {
                case "Multi_Response":
                    customResponse = MessageFactory.Text( answersHolder.Content[RandomSelector.Rand(answersHolder.Content.Count())]);
                    break;
                case "Image_Response":
                    var response = customResponse.CreateReply();
                    try
                    {
                        response.Attachments = new List<Attachment>() { CardMaker.getCardAttachment(CardMaker.getCardJson(answersHolder)) };
                    }
                    catch(Exception e)
                    {
                        telemetryClient.TrackException(e, TelemetryMetaData.BindExceptionEvent(TelemetryKey.CardRenderError, "Image_Response card error",response.Conversation.Id,responseJson));
                    }
                    return response;
                default:
                    customResponse = MessageFactory.Text(answersHolder.Content.First());
                    break;
            }
            return customResponse;

        }
    }
}
