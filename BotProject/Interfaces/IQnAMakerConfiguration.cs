using Microsoft.Bot.Builder.AI.QnA;

namespace YodaBot.BotProject.Interfaces
{
    public interface IQnAMakerConfiguration
    {
        QnAMaker QnAMakerService { get; }
    }
}
