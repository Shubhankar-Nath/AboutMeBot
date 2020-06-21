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
using System.Net.Security;

namespace YodaBot.DataHandler.Processor
{
    public static class ContentSelector
    {
        /// <summary>
        /// To parse the incoming json from QnA Maker in to a reply
        /// </summary>
        /// <param name="customResponse"></param>
        /// <param name="responseJson"></param>
        /// <param name="telemetryClient"></param>
        /// <returns></returns>
        public static Activity getAnswer(Activity customResponse,string responseJson, IBotTelemetryClient telemetryClient) 
        {
            Answers answersHolder = null;
            CardMaker _cardMaker = new CardMaker(telemetryClient);
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
                    try
                    {
                        var response = customResponse.CreateReply();
                        response.Attachments = new List<Attachment>() { _cardMaker.getCardAttachment(_cardMaker.getImage_ResponseCardJson(answersHolder)) };
                        return response;
                    }
                    catch(Exception e)
                    {
                        telemetryClient.TrackException(e, TelemetryMetaData.BindExceptionEvent(TelemetryKey.CardRenderError, "Image_Response card error", customResponse.Conversation.Id,responseJson));
                    }                    
                    break;
                case "Card_Response":
                    try
                    {
                        var response = customResponse.CreateReply();
                        response.Attachments = new List<Attachment>() { _cardMaker.getCardAttachment(_cardMaker.getCard_ResponseCardJson(answersHolder)) };
                        return response;
                    }
                    catch(Exception e)
                    {
                        telemetryClient.TrackException(e, TelemetryMetaData.BindExceptionEvent(TelemetryKey.CardRenderError, "Card_Response card error", customResponse.Conversation.Id, responseJson));
                    }
                    break;
                default:
                    customResponse = MessageFactory.Text(answersHolder.Content.First());
                    break;
            }
            return customResponse;

        }
    }
}
