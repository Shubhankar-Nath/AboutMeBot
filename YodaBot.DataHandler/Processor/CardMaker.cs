using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YodaBot.DataHandler.Models;
using YodaBot.DataHandler.Processor;

namespace YodaBot.DataHandler.Processor
{
    public class CardMaker
    {
        private IBotTelemetryClient _telemetryClient;
        public CardMaker(IBotTelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }
        /// <summary>
        /// For parsing Image data into a card
        /// </summary>
        /// <param name="answerholder"></param>
        /// <returns></returns>
        public string getImage_ResponseCardJson(Answers answerholder)
        {
            JObject templeteJson = null;
            try
            {
                templeteJson = JObject.Parse(File.ReadAllText(Path.Combine(new string[] { ".", @"wwwroot/Sticker_TextCard.json" })));
                //Editing the templete
                templeteJson["body"][0]["items"][0]["url"] = answerholder.Media[RandomSelector.Rand(answerholder.Media.Count())]??="";
                templeteJson["body"][0]["items"][1]["text"] = answerholder.Content[RandomSelector.Rand(answerholder.Content.Count())]??="";                
            }
            catch(Exception e)
            {
                throw e;
            }
            return templeteJson.ToString();
        }
        /// <summary>
        /// Calls the blob to download the json and send
        /// </summary>
        /// <param name="answerholder"></param>
        /// <returns></returns>
        public String getCard_ResponseCardJson(Answers answerholder)
        {
            try
            {
                string parsedURL = answerholder.Content[RandomSelector.Rand(answerholder.Content.Count())];
                using (WebClient webClient = new WebClient())
                {
                    string adaptiveCardJson = webClient.DownloadString(parsedURL);
                    return adaptiveCardJson.Trim();
                }
            }
            catch(Exception e)
            {
                throw e;
            }          
        }
        /// <summary>
        /// Returns an adaptive card as attachment
        /// </summary>
        /// <param name="cardJson"></param>
        /// <returns></returns>
        public Attachment getCardAttachment(string cardJson)
        {
            AdaptiveCard adaptiveCard = AdaptiveCard.FromJson(cardJson).Card;
            var cardAttach = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
            };
            return cardAttach;
        }
    }
}
