using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using YodaBot.DataHandler.Processor;
using YodaBot.Utility.Logging;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class QnABot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly BotState ConversationState;

        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog Dialog;
        protected readonly BotState UserState;
        protected readonly IBotTelemetryClient TelemetryClient;

        public QnABot(ConversationState conversationState, UserState userState, T dialog, IBotTelemetryClient telemetryClient)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            TelemetryClient = telemetryClient;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) =>
            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);

        //When a new chat session starts
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    //Make welcome card
                    Attachment attachmennt = null;
                    try
                    {
                        var cardjson = File.ReadAllText(Path.Combine(new string[] { ".", @"wwwroot/WelcomeCard.json" }));
                        attachmennt = CardMaker.getCardAttachment(cardjson);
                    }
                    catch(Exception e)
                    {
                        TelemetryClient.TrackException(e, TelemetryMetaData.BindExceptionEvent(TelemetryKey.CardRenderError,"Welcome Card Error"));
                    }
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(attachmennt), cancellationToken);
                }
            }
        }
    }
}
