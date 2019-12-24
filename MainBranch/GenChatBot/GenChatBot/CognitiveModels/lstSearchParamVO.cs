using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenChatBot.CognitiveModels
{

    /// <remarks></remarks>
	[Serializable()]
    public class SearchParameterVO
    {
        public List<lstSearchParamVO> LstSearchParam{ get; set; }
        public string UserLanguageCulture { get; set; }
        public string SSOID { get; set; }
        public string VATAccess { get; set; }
    }
    public class lstSearchParamVO
    {
        public string ColumnName { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public string ValueOne { get; set; }
        public string ValueTwo { get; set; }
     }
   
}
