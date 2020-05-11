using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using YodaBot.BotProject.Interfaces;
using YodaBot.Utility.Logging;

namespace Microsoft.BotBuilderSamples.Dialog
{
    /// <summary>
    /// This is an example root dialog. Replace this with your applications.
    /// </summary>
    public class RootDialog : ComponentDialog
    {
        /// <summary>
        /// QnA Maker initial dialog
        /// </summary>
        private const string InitialDialog = "initial-dialog";

        /// <summary>
        /// Initializes a new instance of the <see cref="RootDialog"/> class.
        /// </summary>
        /// <param name="services">Bot Services.</param>
        public RootDialog(IQnAMakerConfiguration services, IBotTelemetryClient telemetryClient)
            : base("root")
        {
            AddDialog(new QnAMakerBaseDialog(services, telemetryClient));

            AddDialog(new WaterfallDialog(InitialDialog)
               .AddStep(InitialStepAsync));

            // The initial child Dialog to run.
            InitialDialogId = InitialDialog;
            // Set the telemetry client for this and all child dialogs.
            this.TelemetryClient = telemetryClient;
        }

    private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set values for generate answer options.
            var qnamakerOptions = new QnAMakerOptions
            {
                ScoreThreshold = QnAMakerBaseDialog.DefaultThreshold,
                Top = QnAMakerBaseDialog.DefaultTopN,
                Context = new QnARequestContext()
            };

            var noAnswer = (Activity)Activity.CreateMessageActivity();
            noAnswer.Text = QnAMakerBaseDialog.DefaultNoAnswer;

            var cardNoMatchResponse = new Activity(QnAMakerBaseDialog.DefaultCardNoMatchResponse);

            // Set values for dialog responses.	
            var qnaDialogResponseOptions = new QnADialogResponseOptions
            {
                NoAnswer = noAnswer,
                ActiveLearningCardTitle = QnAMakerBaseDialog.DefaultCardTitle,
                CardNoMatchText = QnAMakerBaseDialog.DefaultCardNoMatchText,
                CardNoMatchResponse = cardNoMatchResponse
            };

            var dialogOptions = new Dictionary<string, object>
            {
                [QnAMakerBaseDialog.QnAOptions] = qnamakerOptions,
                [QnAMakerBaseDialog.QnADialogResponseOptions] = qnaDialogResponseOptions
            };

            return await stepContext.BeginDialogAsync(nameof(QnAMakerBaseDialog), dialogOptions, cancellationToken);
        }
    }
}