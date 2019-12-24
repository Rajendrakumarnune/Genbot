// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.6.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using GenChatBot.Models;

using GenChatBot.CognitiveModels;
using System.Net.Http;
using Newtonsoft.Json;

namespace GenChatBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly FlightBookingRecognizer _luisRecognizer;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(FlightBookingRecognizer luisRecognizer, BookingDialog bookingDialog, CustomerDialog customer, CustomerAccountBalanceDialog customerAccount, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(bookingDialog);
            AddDialog(customer);
            AddDialog(customerAccount);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }
        private static async Task<LuisModel> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LuisModel Data = new LuisModel();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/b2e8397f-e9e7-458b-848d-abc4da3d51b4?verbose=true&timezoneOffset=0&subscription-key=47f4a7a581cd43d2921cfd63e040a3e4&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject(JsonDataResponse);

                    //Data = JsonConvert.DeserializeObject<LuisModel>(JsonDataResponse);
                    Data = JsonConvert.DeserializeObject<LuisModel>(JsonDataResponse.ToString());
                    return Data;
                }
                return Data;
            }

        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //if (!_luisRecognizer.IsConfigured)
            //{
            //    // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
            //    return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            //}
            //var CustomerBalance1 = new CustomerDetailsModel()
            //{
            //    // Get destination and origin from the composite entities arrays.
            //   // DebtorName = luisResult.Entities.DebtorName,
            //    DebtorNumber = "0001610280"

            //};
            // return await stepContext.BeginDialogAsync(nameof(CustomerDialog), CustomerBalance1, cancellationToken);
            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            LuisModel StLUIS = await GetEntityFromLUIS(stepContext.Result.ToString());
            if (StLUIS.topScoringIntent.intent == "CustomerStatement")
            {
                var customerDetails = new CustomerDetailsModel();
                if (StLUIS.entities[0].type == "DebtorNumber")
                {
                    customerDetails.DebtorNumber = StLUIS.entities[0].entity;
                }
                else if (StLUIS.entities[0].type == "DebtorName")
                {
                    customerDetails.DebtorName = StLUIS.entities[0].entity;
                }
                return await stepContext.BeginDialogAsync(nameof(CustomerAccountBalanceDialog), customerDetails, cancellationToken);
            }
            else if (StLUIS.topScoringIntent.intent == "CustomerAccountBalance")
            {
                var customerDetails = new CustomerDetailsModel();
                if (StLUIS.entities.Count > 0)
                {
                    if (StLUIS.entities[0].type == "DebtorNumber")
                    {
                        customerDetails.DebtorNumber = StLUIS.entities[0].entity;
                    }
                    else if (StLUIS.entities[0].type == "DebtorName")
                    {
                        customerDetails.DebtorName = StLUIS.entities[0].entity;
                    }
                }
                return await stepContext.BeginDialogAsync(nameof(CustomerDialog), customerDetails, cancellationToken);

            }
            else if (StLUIS.topScoringIntent.intent == "Statement Of Account")
            {
                var customerDetails = new CustomerDetailsModel();
                if (StLUIS.entities.Count > 0)
                {
                    if (StLUIS.entities[0].type == "DebtorNumber")
                    {
                        customerDetails.DebtorNumber = StLUIS.entities[0].entity;
                    }
                    else if (StLUIS.entities[0].type == "DebtorName")
                    {
                        customerDetails.DebtorName = StLUIS.entities[0].entity;
                    }
                }
                return await stepContext.BeginDialogAsync(nameof(CustomerDialog), customerDetails, cancellationToken);
            }
            else if (StLUIS.topScoringIntent.intent == "Goodbye")
            {
                var endMessageText = $"good bye";
                var endMessage = MessageFactory.Text(endMessageText, endMessageText, InputHints.IgnoringInput);

                return await stepContext.ReplaceDialogAsync(InitialDialogId, endMessageText, cancellationToken);
            }
            else
            {
                var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way ";
                       var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
            }

                //var luisResult = await _luisRecognizer.RecognizeAsync<FlightBooking>(stepContext.Context, cancellationToken);
                //switch (luisResult.TopIntent().intent)
                //{
                //    case FlightBooking.Intent.CustomerAccountBalance:
                //       // await ShowWarningForUnsupportedCities(stepContext.Context, luisResult, cancellationToken);

                //        //// Initialize BookingDetails with any entities we may have found in the response.
                //        //var bookingDetails = new BookingDetails()
                //        //{
                //        //    // Get destination and origin from the composite entities arrays.
                //        //    Destination = luisResult.ToEntities.Airport,
                //        //    Origin = luisResult.FromEntities.Airport,
                //        //    TravelDate = luisResult.TravelDate,
                //        //};

                //        var CustomerBalance = new CustomerDetailsModel()
                //        {
                //            // Get destination and origin from the composite entities arrays.
                //            DebtorName = luisResult.Entities.DebtorName,
                //            DebtorNumber = luisResult.Entities.DebtorNumber

                //        };

                //        // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                //        return await stepContext.BeginDialogAsync(nameof(CustomerAccountBalanceDialog), CustomerBalance, cancellationToken);

                //    case FlightBooking.Intent.CustomerStatement:
                //        // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                //       // var getWeatherMessageText = "TODO: get weather flow here";
                //       // var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                //       // await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);

                //        var CustomerDetails = new CustomerDetailsModel()
                //        {
                //            // Get destination and origin from the composite entities arrays.
                //            DebtorName = luisResult.Entities.DebtorName,
                //            DebtorNumber = luisResult.Entities.DebtorNumber

                //        };
                //        // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                //        return await stepContext.BeginDialogAsync(nameof(CustomerDialog), CustomerDetails, cancellationToken);
                //        break;

                //    default:
                //        // Catch all for unhandled intents
                //        var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                //        var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                //        await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                //        break;
                //}

                return await stepContext.NextAsync(null, cancellationToken);
        }

        // Shows a warning if the requested From or To cities are recognized as entities but they are not in the Airport entity list.
        // In some cases LUIS will recognize the From and To composite entities as a valid cities but the From and To Airport values
        // will be empty if those entity values can't be mapped to a canonical item in the Airport.
        private static async Task ShowWarningForUnsupportedCities(ITurnContext context, FlightBooking luisResult, CancellationToken cancellationToken)
        {
            var unsupportedCities = new List<string>();

            var fromEntities = luisResult.FromEntities;
            if (!string.IsNullOrEmpty(fromEntities.From) && string.IsNullOrEmpty(fromEntities.Airport))
            {
                unsupportedCities.Add(fromEntities.From);
            }

            var toEntities = luisResult.ToEntities;
            if (!string.IsNullOrEmpty(toEntities.To) && string.IsNullOrEmpty(toEntities.Airport))
            {
                unsupportedCities.Add(toEntities.To);
            }

            if (unsupportedCities.Any())
            {
                var messageText = $"Sorry but the following airports are not supported: {string.Join(',', unsupportedCities)}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await context.SendActivityAsync(message, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is BookingDetails result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
