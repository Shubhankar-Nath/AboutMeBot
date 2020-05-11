using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaBot.BotProject.Interfaces;

namespace YodaBot.BotProject
{
    public class QnAMakerConfiguration : IQnAMakerConfiguration
    {
        public QnAMakerConfiguration(IConfiguration configuration, IBotTelemetryClient telemetryClient)
        {
            QnAMakerService = new QnAMaker(new QnAMakerEndpoint
            {
                KnowledgeBaseId = configuration["QnAKnowledgebaseId"],
                EndpointKey = configuration["QnAEndpointKey"],
                Host = GetHostname(configuration["QnAEndpointHostName"])
            },
            null,
            null,
            telemetryClient: telemetryClient);
        }

        public QnAMaker QnAMakerService { get; private set; }

        private static string GetHostname(string hostname)
        {
            if (!hostname.StartsWith("https://"))
            {
                hostname = string.Concat("https://", hostname);
            }

            if (!hostname.EndsWith("/qnamaker"))
            {
                hostname = string.Concat(hostname, "/qnamaker");
            }

            return hostname;
        }
    }
}
