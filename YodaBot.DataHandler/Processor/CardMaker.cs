using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YodaBot.DataHandler.Models;
using YodaBot.DataHandler.Processor;

namespace YodaBot.DataHandler.Processor
{
    public static class CardMaker
    {
        public static string getCardJson(Answers answerholder)
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
                Console.WriteLine(e.Message);
            }
            return templeteJson.ToString();
        }

        public static Attachment getCardAttachment(string cardJson)
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
