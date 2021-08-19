﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class TileCard2 : Widget
    {
        /// <summary>
        /// Actual Peroperties that holds the values
        /// </summary>
        public decimal Count { get; set; }
        public decimal Amount { get; set; }
        public string Format { get; set; }
        public string Link { get; set; }
        public string Filter { get; set; }


        /// <summary>
        /// Peropeties that holds the above values columns Name
        /// </summary>
        public string CountColumnName { get; set; }
        public string AmountColumnName { get; set; }
        public string FilterColumnName { get; set; }

        public string Query
        {
            get
            {
                if (!string.IsNullOrEmpty(FilterColumnName) && !string.IsNullOrEmpty(Filter))
                {
                    return "SELECT " + CountColumnName + ", " + AmountColumnName + " from " + base.SourceName + " where "+ FilterColumnName+" ='"+Filter+"'";
                }
                else
                {
                    return "SELECT " + CountColumnName + ", " + AmountColumnName + " from " + base.SourceName;
                }
            }
        }
    }
}