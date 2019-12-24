using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusinessLayer.CommonClass;

namespace BusinessLayer
{
    public class CollectionServiceBL
    {
        public static async Task<List<CustomerModel>> GetServiceResponse(string CustomerNo, string customerName )
        {
            List<CustomerModel> objCustDetails = new List<CustomerModel>();
            SearchParameterVO objSearch = null;
            if (CustomerNo == null)
            {
                objSearch = new SearchParameterVO();
                objSearch = CommonClasses.GetByCustomerNumber(CustomerNo);
                
            }
            else
            {
                objSearch = new SearchParameterVO();
                objSearch = CommonClasses.GetByCustomerName(customerName);
            }
          
            HttpResponseMessage responses = null;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           
            //var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{pass}"));
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            //httpClient.Timeout = TimeSpan.FromMinutes(5);
            //for (int i = 0; i < 5; i++)
            //{
           // string message = string.Empty;

            //http://localhost:55769/API/Collection/api/Search/Filter/Customer/12/503106097
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(objSearch), Encoding.UTF8, "application/json");
                FakeService fakeService = new FakeService();
                List<ServiceModel> data = await fakeService.GetFakeData(CustomerNo);
                for (int i = 0; i < data.Count; i++)
                {
                   
                       objCustDetails = new List<CustomerModel> { new CustomerModel
                        { CustomerName = data[i].Name ,
                          CustomerNumber = data[i].CustomerNo,
                          Balance = data[i].Balance
                        } };
                    //balance = resultData.data.aaData[i].Balance;
                    //no = resultData.data.aaData[i].CustomerNo;
                    //name = resultData.data.aaData[i].Name;
                }
                return objCustDetails;

                //// responses = await httpClient.PostAsync("http://localhost:55769/API/Collection/api/Search/Customer", content);
                //if (responses.IsSuccessStatusCode)
                //{
                //    objCustDetails = new List<CustomerModel>();
                //    var resp = await responses.Content.ReadAsStringAsync();
                //    var resultData = JsonConvert.DeserializeObject<MainModel>(resp.ToString());

                //    //string balance = string.Empty;
                //    //string no = string.Empty;
                //    //string name = string.Empty;
                //    for (int i = 0; i < resultData.data.aaData.Count; i++)
                //    {
                //        objCustDetails = new List<CustomerModel> { new CustomerModel
                //        { CustomerName = resultData.data.aaData[i].Name ,
                //          CustomerNumber = resultData.data.aaData[i].CustomerNo,
                //          Balance = resultData.data.aaData[i].Balance
                //        } };
                //        //balance = resultData.data.aaData[i].Balance;
                //        //no = resultData.data.aaData[i].CustomerNo;
                //        //name = resultData.data.aaData[i].Name;
                //    }

                //    // message ="Customer Name :"+ name+" Customer No :" + no+" Balance :"+balance;                  
                //    return objCustDetails.ToString();     //message;
                //}
                //else
                //{
                //    return objCustDetails.ToString();
                //}    
            }

            catch (Exception e)
            {
                return objCustDetails;
            }                  
        }
    }
}
