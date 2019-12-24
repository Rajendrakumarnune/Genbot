using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Model;
using GenChatBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json;

namespace GenChatBot.Dialogs
{
    public class CustomerAccountBalanceDialog : CancelAndHelpDialog
    {
        private const string CustomerNoStepMsgText = "Please provide customer Name";

        public CustomerAccountBalanceDialog()
             : base(nameof(CustomerAccountBalanceDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            //AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                WelcomeStepAsync,
                //OriginStepAsync,
                //TravelDateStepAsync,
                ConfirmStepAsync,
                ProcessStepAsync
               // FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> WelcomeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var customerBalance = (CustomerDetailsModel)stepContext.Options;

            if (customerBalance.DebtorName == null)
            {
                var promptMessage = MessageFactory.Text(CustomerNoStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(customerBalance.DebtorNumber, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var customerDetails = (CustomerDetailsModel)stepContext.Options;
            if(customerDetails.DebtorName == null)
                customerDetails.DebtorName = (string)stepContext.Result;



            var messageText = $"Please confirm, you have asked: {customerDetails.DebtorName}. Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }
        private async Task<DialogTurnResult> ProcessStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Need to call service inside if condition TODO...
            if ((bool)stepContext.Result)
            {
                ServiceResponceModel lstServiceResult = new ServiceResponceModel();
                List<ServiceModel> lstService = new List<ServiceModel>();
               // lstServiceResult.LstServiceResp   = new List<ServiceModel>();

                ServiceModel s = new ServiceModel();

               // lstServiceResult.LstServiceResp.Add(s);
                

             //   lstServiceResult.LstServiceResp.Add(resultData);
              var customerDetails = (CustomerDetailsModel)stepContext.Options;

                var response = await CollectionServiceBL.GetServiceResponse(customerDetails.DebtorNumber,customerDetails.DebtorName);
                var resultData = JsonConvert.DeserializeObject<ServiceModel>(response.ToString());

                var messageText = $"Customer Account balance is in Inprocess";
                var promptMessage = MessageFactory.Text(messageText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            CustomerDetailsModel c = new CustomerDetailsModel();
            c.DebtorNumber = null;
            //return await stepContext.ReplaceDialogAsync(InitialDialogId, c, cancellationToken);
             return await stepContext.NextAsync(null, cancellationToken);
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var bookingDetails = (CustomerDetailsModel)stepContext.Options;

                return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            }
            else
            {
                var promptMessage = MessageFactory.Text(CustomerNoStepMsgText, CustomerNoStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                //return await stepContext.EndDialogAsync(stepContext, cancellationToken);
                //return await stepContext.ReplaceDialogAsync(InitialDialogId, CustomerNoStepMsgText, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
    