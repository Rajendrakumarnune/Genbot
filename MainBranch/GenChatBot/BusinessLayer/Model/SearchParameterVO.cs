using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Model
{
    public class SearchParameterVO
    {

        public List<LstSearchParamVO> lstSearchParamVO { get; set; }
        public string UserLanguageCulture { get; set; }
        public string SSOID { get; set; }
        public string VATAccess { get; set; }
    }

    public class LstSearchParamVO
    {
        public string ColumnName { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public string ValueOne { get; set; }
        public string ValueTwo { get; set; }
    }
}
