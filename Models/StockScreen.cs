using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_TillRemote.Models
{
    public class StockScreen
    {
           // public int LblID { get; set; }
            public string LblName { get; set; }
            public int FieldLength { get; set; }
            public string IsActive { get; set; }
            public string PropertyValue { get; set; }
            public string FieldType { get; set; }
            public string ReadOnlyProp { get; set; }
            public string FieldName { get; set; }
        
    }

    public class Section1
    {
        public string CODE { get; set; }
        public string NAME { get; set; }

    }

    public class Section2
    {
        public string CODE { get; set; }
        public string NAME { get; set; }

    }

    public class AddStock
    {
        public string ITEMCODE { get; set; }
        public string DESCRPTON1 { get; set; }
        public string DESCRPTON2 { get; set; }
        public string SUPPLIER { get; set; }
        public double Q_IN_HAND { get; set; }
        public string SECTION1 { get; set; }
        public string SECTION2 { get; set; }
        public string SECTION3 { get; set; }
        public string SECTION4 { get; set; }
        public string SECTION5 { get; set; }

        public string SDATE_FROM { get; set; }
        public string SDATE_TO { get; set; }

        public double T1_RATE { get; set; }
        public double RO_LEVEL { get; set; }
        public double COST { get; set; }
        public double LAST_COST { get; set; }

        public double B_RETAIL { get; set; }
        public double B_RETAILInclude { get; set; }

        public double OH_SORDER { get; set; }
       


    }



   
}
