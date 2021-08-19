using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class WidgetStructure
    {
        public int ID { get; set; }
        public int ElementID { get; set; }
        public string ClassType { get; set; }
        public string Formation { get; set; }
        public bool IsDeActivated { get; set; }
    }
}
