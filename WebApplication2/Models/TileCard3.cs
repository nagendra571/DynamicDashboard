using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class TileCard3 : Widget
    {
        /// <summary>
        /// Actual Peroperties that holds the values
        /// </summary>
        public decimal PlanedValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal PerformanceValue { get { return this.ActualValue - this.PlanedValue; } }

        public string PlanedValueFormat { get; set; }
        public string ActualValueFormat { get; set; }
        public string PerformanceValueFormat { get; set; }

        public string ActualValueLink { get; set; }


        /// <summary>
        /// Peropeties that holds the above values columns Name
        /// </summary>
        public string PlanedValueColumnName { get; set; }
        public string ActualValueColumnName { get; set; }
        public string FilterColumnName { get; set; }
        public string Filter { get; set; }

        public string Query
        {
            get
            {
                string Query = "SELECT " + PlanedValueColumnName + ", " + ActualValueColumnName;

                if (!string.IsNullOrEmpty(base.AsOfDateColumnName))
                {
                    Query = Query + ", " + base.AsOfDateColumnName;
                }

                Query = Query + " from " + base.SourceName;

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
}
