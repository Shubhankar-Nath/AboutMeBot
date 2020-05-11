using Microsoft.Bot.Builder.AI.QnA;

using Microsoft.Bot.Builder.AI.QnA;

namespace YodaBot.BotProject.Interfaces
{
    public interface IQnAMakerConfiguration
    {
        QnAMaker QnAMakerService { get; }
    }
}
