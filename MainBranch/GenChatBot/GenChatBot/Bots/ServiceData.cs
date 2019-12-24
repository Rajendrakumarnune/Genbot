using GenChatBot.CognitiveModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenChatBot.Bots
{
    public static class ServiceData
    {
        public static async Task<string> GetServiceResponse()
        {
           
            SearchParameterVO objSearch = new SearchParameterVO();
            List<lstSearchParamVO> objSerchParam = new List<lstSearchParamVO>();
            objSerchParam = new List<lstSearchParamVO> { new lstSearchParamVO { ColumnName = "Customer.cust_no_717",
                Name = "Customer Number",
                Condition = "Equals",
                ValueOne = "0001610280",
                ValueTwo = null
            }
          }; objSearch.UserLanguageCulture = "en-US";
            objSearch.SSOID = "503106097";
            objSearch.VATAccess = false.ToString();

            objSearch.LstSearchParam = objSerchParam;
            HttpResponseMessage responses = null;
            string str = string.Empty; 
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:55769/API");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            string userName = "503106097";
            string pass = "HS@1716GE";
            string UserViewLanguage = "en-US";
            string CustomerNo = "0001610280";
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{pass}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            httpClient.Timeout = TimeSpan.FromMinutes(5);
        //for (int i = 0; i < 5; i++)
        //{

        //http://localhost:55769/API/Collection/api/Search/Filter/Customer/12/503106097
            try
                {
                  var response = httpClient.GetStringAsync(" http://localhost:55769/API/Collection/api/Search/Filter/Customer/12/503106097");
                  response.Wait(TimeSpan.FromMinutes(3));
                  var results = response.Result;
                    //if (i == 5)
                    //{
                        return results.ToString();
                    //}
                    //i += 1;
                    Thread.Sleep(10000);
                }

                catch (Exception e)
                {
                    return e.Message.ToString();
                }
                
            //}

            /*  try
              {             

                  var client = new HttpClient();
                  client.BaseAddress = new Uri("http://localhost:55769/API");
                  client.DefaultRequestHeaders.Accept.Clear();
                  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                  var content = new StringContent(JsonConvert.SerializeObject(objSearch), Encoding.UTF8, "application/json");

                  using (var response = await client.PostAsync("/Collection/Search/Filter/Customer/", content))
                  {
                      Thread.Sleep(10000);
                      if (response.IsSuccessStatusCode)
                      {
                          var resp = await response.Content.ReadAsStringAsync();
                          // Get the URI of the created resource.  
                          Uri returnUrl = response.Headers.Location;
                          Console.WriteLine(returnUrl);
                          return JObject.Parse(resp);
                      }
                      else
                          return null;
                  }

                  //HttpResponseMessage responses = await client.PostAsync("/Collection/Search/Filter/Customer/", content);
                  //if (response.IsSuccessStatusCode)
                  //{
                  //    var resp = await response.Content.ReadAsStringAsync();
                  //    return JObject.Parse(resp);
                  //}
                  //else
                  //    return null;

              }
              catch (Exception ex)
              {
                  return null;
              } */
            return string.Empty;
        }


        //public async Task<HttpResponseMessage> SendRequestAsync()
        //{
        //    SearchParameterVO objSearch = new SearchParameterVO();
        //    List<lstSearchParamVO> objSerchParam = new List<lstSearchParamVO>();
        //    objSerchParam = new List<lstSearchParamVO> { new lstSearchParamVO { ColumnName = "Customer.cust_no_717",
        //        Name = "Customer Number",
        //        Condition = "Equals",
        //        ValueOne = "0001610280",
        //        ValueTwo = null
        //    }
        //  }; objSearch.UserLanguageCulture = "en-US";
        //    objSearch.SSOID = "503106097";
        //    objSearch.VATAccess = false.ToString();

        //    objSearch.LstSearchParam = objSerchParam;


        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        //StringContent httpConent = new StringContent(xmlRequest, Encoding.UTF8);
        //        //var client = new HttpClient();
        //        httpClient.BaseAddress = new Uri("http://localhost:55769/API");
        //        httpClient.DefaultRequestHeaders.Accept.Clear();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        //        var content = new StringContent(JsonConvert.SerializeObject(objSearch), Encoding.UTF8, "application/xml");

        //       HttpResponseMessage responseMessage = null;
        //        try
        //        {
        //            responseMessage = await httpClient.PostAsync("Collection/Search/Filter/Customer/12/703185080", content);
        //            Thread.Sleep(10000);
        //            //return responseMessage;
        //        }
        //        catch (Exception ex)
        //        {
        //            if (responseMessage == null)
        //            {
        //                responseMessage = new HttpResponseMessage();
        //            }
        //            responseMessage.StatusCode = HttpStatusCode.InternalServerError;
        //            responseMessage.ReasonPhrase = string.Format("RestHttpClient.SendRequest failed: {0}", ex);
        //        }
        //        return responseMessage;
        //    }
        //}
    }
}
