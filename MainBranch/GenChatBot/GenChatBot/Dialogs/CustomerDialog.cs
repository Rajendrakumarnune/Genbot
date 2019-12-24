
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

namespace GenChatBot.Dialogs
{
    public class CustomerDialog : CancelAndHelpDialog
    {
        private const string CustomerNoStepMsgText = "Please provide customer No";
        //private const string OriginStepMsgText = "Where are you traveling from?";

        public CustomerDialog()
           : base(nameof(CustomerDialog))
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
                //FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

            private async Task<DialogTurnResult> WelcomeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var customerDetails = (CustomerDetailsModel)stepContext.Options;

                if (customerDetails.DebtorNumber == null)
                {
                    var promptMessage = MessageFactory.Text (CustomerNoStepMsgText, InputHints.ExpectingInput);
                    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                }

                return await stepContext.NextAsync(customerDetails.DebtorNumber, cancellationToken);
            }

            private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
                var customerDetails = (CustomerDetailsModel)stepContext.Options;
            
                if (customerDetails.DebtorNumber == null)
                    customerDetails.DebtorNumber = (string)stepContext.Result;
           

                var messageText = $"Please confirm, you have asked: {customerDetails.DebtorNumber}. Is this correct?";
                var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

                return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
        private async Task<DialogTurnResult> ProcessStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            CustomerDetailsModel c = new CustomerDetailsModel();
            var customerDetails = (CustomerDetailsModel)stepContext.Options;
            var messageText = string.Empty;
            string balance = string.Empty;
            string no = string.Empty;
            string name = string.Empty;
            // c.DebtorName = null;
            //Need to call service inside if condition TODO...
            if ((bool)stepContext.Result)
            {
                List<CustomerModel> response = await CollectionServiceBL.GetServiceResponse(customerDetails.DebtorNumber,customerDetails.DebtorName);
                for (int i = 0; i < response.Count; i++) {
                    balance = response[i].Balance;
                    no = response[i].CustomerNumber;
                    name = response[i].CustomerName;

                }
                 messageText = "Customer Name :" + name + " Customer No :" + no + " Balance :" + balance;

                if (messageText == "" || messageText == null) {
                    messageText = "We coundn't find any customer no "+ customerDetails.DebtorNumber;
                }
                var promptMessage = MessageFactory.Text(messageText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
               // return await stepContext.NextAsync(null, cancellationToken);
            }
            else { return await stepContext.ReplaceDialogAsync(InitialDialogId,c, cancellationToken); }
           
            //return await stepContext.ReplaceDialogAsync(InitialDialogId, c, cancellationToken);
            
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
            if ((bool)stepContext.Result)
            {
                var customerDetail = (CustomerDetailsModel)stepContext.Options;

                return await stepContext.EndDialogAsync(customerDetail, cancellationToken);
            }
            else {
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

