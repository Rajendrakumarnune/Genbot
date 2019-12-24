using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Model
{
    public class MainModel
    {
        public ServiceResponceModel data { get; set; }

    }
   public class ServiceResponceModel

    {
        public string iTotalRecords { get; set; }
         public List<ServiceModel> aaData { get; set; }
    }
    public class JsonData
    {
        // public int iTotalRecords { get; set; }

        public List<ServiceModel> aaData { get; set; }
    }
    public class ServiceModel
    {
        public string OpenItems { get; set; }
        public string CollectorName { get; set; }
        public string PhoneNo { get; set; }
        public string Ssoid { get; set; }
        public string Name { get; set; }
        public string CustomerNo { get; set; }
        public string BusinessAlias { get; set; }
        public string Status1 { get; set; }
        public string Balance { get; set; }
        public string BillingCurrency { get; set; }
        public string CCNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Definer { get; set; }
        public string ParentSub { get; set; }
        public string PLName { get; set; }
        public string ExtendedName { get; set; }
        public string NativeAddress { get; set; }
        public string OrgId { get; set; }
        public string IsSensitiveCustomer { get; set; }
        public string CaseId { get; set; }
    }
}
