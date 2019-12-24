using BusinessLayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class FakeService
    {

        public  async Task<List<ServiceModel>> GetFakeData(string CustomerNo)
        {
            CustomerNo = Uri.EscapeDataString(CustomerNo);
            List<ServiceModel> Data = new List<ServiceModel>();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://my-json-server.typicode.com/Rajendrakumarnune/FakeService/aaData?CustomerNo="+CustomerNo;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject(JsonDataResponse);
                    //JsonDataResponse = JsonDataResponse.Replace("[", "").Replace("]", "");
                    //Data = JsonConvert.DeserializeObject<LuisModel>(JsonDataResponse);
                    Data = JsonConvert.DeserializeObject<List<ServiceModel>>(JsonDataResponse.ToString());
                    return Data;
                   
                }
                return Data;
            }

        }
    }
}
