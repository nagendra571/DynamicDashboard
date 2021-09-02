using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class PieChart : Widget
    {
        /// <summary>
        /// Actual Peroperties that holds the values
        /// </summary>
        public string Category { get; set; }
        public string Value { get; set; }


        /// <summary>
        /// Peropeties that holds the above values columns Name
        /// </summary>
        public string CategoryColumnName { get; set; }
        public string ValueColumnName { get; set; }
        public string LabelTemplateFormat { get; set; }


        public string FilterColumnName { get; set; }
        public string Filter { get; set; }



        public IEnumerable<PieChartRecord> Data{ get; set; }

        public string Query
        {
            get
            {
                string Query = "SELECT " + CategoryColumnName + ", " + ValueColumnName + " from " + base.SourceName;
                bool isAnd = false;
                if (!string.IsNullOrEmpty(FilterColumnName) && !string.IsNullOrEmpty(Filter))
                {
                    Query = Query + (!isAnd ? " where " : " and ") + FilterColumnName + " ='" + Filter + "'";
                    isAnd = true;
                }
                return Query;
            }
        }


    }

    public class PieChartRecord
    {
        public string Category { get; set; }
        public string Value { get; set; }
        public string Colour { get; set; }
    }
}
