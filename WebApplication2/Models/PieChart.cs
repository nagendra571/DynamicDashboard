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
        public string Colour { get; set; }


        /// <summary>
        /// Peropeties that holds the above values columns Name
        /// </summary>
        public string CategoryColumnName { get; set; }
        public string ValueColumnName { get; set; }
        public string ColourColumnName { get; set; }

        public dynamic[] Data{ get; set; }

    }
}
