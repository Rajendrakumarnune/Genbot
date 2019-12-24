// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.6.2

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenChatBot.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BusinessLayer;

namespace GenChatBot.Bots
{
    // This IBot implementation can run any type of Dialog. The use of type parameterization is to allows multiple different bots
    // to be run at different endpoints within the same project. This can be achieved by defining distinct Controller types
    // each with dependency on distinct IBot types, this way ASP Dependency Injection can glue everything together without ambiguity.
    // The ConversationState is used by the Dialog system. The UserState isn't, however, it might have been used in a Dialog implementation,
    // and the requirement is that all BotState objects are saved at the end of a turn.
    public class DialogBot<T> : ActivityHandler
        where T : Dialog
    {
        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;
        protected readonly ILogger Logger;

        public DialogBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            Logger = logger;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
                      
           
         // var  response = await CollectionServiceBL.GetServiceResponse();    //await objService.SendRequestAsync(); 
            //Thread.Sleep(1000);

            await base.OnTurnAsync(turnContext, cancellationToken);
            //await turnContext.SendActivityAsync($"GE");
            #region commented code..
            //    SearchParameterVO objSearch = new SearchParameterVO();
            //    List<lstSearchParamVO> objSerchParam = new List<lstSearchParamVO>();
            //    objSerchParam = new List<lstSearchParamVO> { new lstSearchParamVO { ColumnName = "Customer.cust_no_717",
            //        Name = "Customer Number",
            //        Condition = "Equals",
            //        ValueOne = "0001610280",
            //        ValueTwo = null
            //}
            //  }; objSearch.UserLanguageCulture = "en-US";
            //    objSearch.SSOID = "503106097";
            //    objSearch.VATAccess = false.ToString();

            //    objSearch.LstSearchParam = objSerchParam;
            //    try
            //    {
            //        //using (HttpClient client = new HttpClient())
            //        //{
            //        //    string RequestURI = "http://localhost:55769/API/Collection/Search/Filter/Customer/12/0001610280";
            //        //    HttpResponseMessage msg = await client.GetAsync(RequestURI);
            //        //    string str = msg.ToString();
            //        //    if (msg.IsSuccessStatusCode)
            //        //    {
            //        //        var JsonDataResponse = await msg.Content.ReadAsStringAsync();
            //        //        //Data = JsonConvert.DeserializeObject<StockLUIS>(JsonDataResponse);
            //        //    }
            //        //}

            //        var client = new HttpClient();
            //        client.BaseAddress = new Uri("http://localhost:55769/API");
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        var content = new StringContent(JsonConvert.SerializeObject(objSearch), Encoding.UTF8, "application/json");

            //        HttpResponseMessage response = await client.PostAsync("/Collection/Search/Filter/Customer/", content);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            Console.WriteLine("Data posted");
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Failed to poste data. Status code:{response.StatusCode}");
            //        }

            //    }
            //    catch (Exception ex)
            //    { }
            // Save any state changes that might have occured during the turn.
            #endregion

            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
           // await turnContext.SendActivityAsync("Test");
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Running dialog with Message Activity.");
          
            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
        }
    }
}
