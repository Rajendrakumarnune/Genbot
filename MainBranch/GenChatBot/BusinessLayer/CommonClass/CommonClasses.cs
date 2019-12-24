using BusinessLayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BusinessLayer.CommonClass
{
   public static class CommonClasses
    {
        public static SearchParameterVO GetByCustomerName( string customerName)
        {
            List<CustomerModel> objCustName = new List<CustomerModel>();
            ServiceResponceModel lstServiceResult = new ServiceResponceModel();
            List<ServiceModel> lstService = new List<ServiceModel>();           
            SearchParameterVO objSearch = new SearchParameterVO();
            List<LstSearchParamVO> objSerchParam = new List<LstSearchParamVO>();
            objSerchParam = new List<LstSearchParamVO> { new LstSearchParamVO { ColumnName = "Cust_long_name_717",
                    Name = "Customer Name",
                    Condition = "Equals",
                    ValueOne =customerName,
                    ValueTwo = null
                }
              }; objSearch.UserLanguageCulture = "en-US";
            objSearch.SSOID = "503106097";
            objSearch.VATAccess = false.ToString();
            objSearch.lstSearchParamVO = objSerchParam;
            return objSearch;
        }

        public static SearchParameterVO GetByCustomerNumber(string customerNo)
        {
            List<CustomerModel> objCustName = new List<CustomerModel>();
            ServiceResponceModel lstServiceResult = new ServiceResponceModel();
            List<ServiceModel> lstService = new List<ServiceModel>();
            SearchParameterVO objSearch = new SearchParameterVO();
            List<LstSearchParamVO> objSerchParam = new List<LstSearchParamVO>();
            objSerchParam = new List<LstSearchParamVO> { new LstSearchParamVO { ColumnName = "Customer.cust_no_717",
                    Name = "Customer NUmber",
                    Condition = "Equals",
                    ValueOne =customerNo,
                    ValueTwo = null
                }
              }; objSearch.UserLanguageCulture = "en-US";
            objSearch.SSOID = "503106097";
            objSearch.VATAccess = false.ToString();
            objSearch.lstSearchParamVO = objSerchParam;
            return objSearch;
        }
    }
}
