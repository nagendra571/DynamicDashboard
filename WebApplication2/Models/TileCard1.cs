using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class TileCard1 : Widget
    {
        /// <summary>
        /// Actual Peroperties that holds the values
        /// </summary>
        public decimal Value { get; set; }
        public decimal PerformanceValue { get; set; }
        public string Format { get; set; }
        public string Link { get; set; }
        public string Filter { get; set; }


        /// <summary>
        /// Peropeties that holds the above values columns Name
        /// </summary>
        public string ValueColumnName { get; set; }
        public string PerformanceValueColumnName { get; set; }
        public string FilterColumnName { get; set; }

        public string Query
        {
            get
            {
                string Query = "SELECT " + ValueColumnName + ", " + PerformanceValueColumnName + " from " + base.SourceName;
                bool isAnd = false;
                if (!string.IsNullOrEmpty(FilterColumnName) && !string.IsNullOrEmpty(Filter))
                {
                    Query = Query + (!isAnd ? " where " : " and ") + FilterColumnName + " ='" + Filter + "'";
                    isAnd = true;
                }
                //if (base.isFilteredByUserId)
                //{
                //    Query = Query + (!isAnd ? " where UserEmail = '" : " and UserEmail = '") + base.UserID +"'";
                //    isAnd = true;
                //}
                //if (base.isFilteredByRole)
                //{
                //    Query = Query + (!isAnd ? " where RoleID = " : " and RoleID = ") + base.RoleID;
                //}
                return Query;
            }
        }

    }
}
