using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Widget
    {
        public int ElementTemplateID { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public bool IsRealValues { get; set; }
        public bool RequiredCaptureValues { get; set; }
        public string SourceName { get; set; }       


        public Dictionary<string, IEnumerable<string>> TableAndColumns { get; set; }
    }
}
