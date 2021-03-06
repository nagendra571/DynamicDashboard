using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Widget
    {
        public int WidgetID { get; set; }
        public int DashboardID { get; set; }
        public int ElementTemplateID { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string SourceName { get; set; }
        public string AsOfDateColumnName { get; set; }
        public DateTime? AsOfDateValue { get; set; }


        public bool IsRealValues { get; set; }
        public bool WidgetSelection { get; set; }
        public bool RequiredCaptureValues { get; set; }
        public bool IsAccessble { get; set; }
        public bool IsDefaulted { get; set; }
        public bool IsDragDropEnabled { get; set; }

        public bool isFilteredByUserId { get; set; }
        public bool isFilteredByRole { get; set; }


        public string UserID { get; set; }
        public int RoleID { get; set; }

        public int Position { get; set; }
        public string SaveActionName
        {
            get
            {
                string actionName = "";
                switch (this.ElementTemplateID)
                {
                    case 7:
                        actionName = "SaveTileCard1";
                        break;
                    case 6:
                        actionName = "SaveTileCard2";
                        break;
                    case 5:
                    case 4:
                    case 3:
                        actionName = "SavePieChart";
                        break;
                    case 2:
                        actionName = "SaveTileCard3";
                        break;
                        

                }
                return actionName;
            }
        }


        public Dictionary<string, IEnumerable<string>> TableAndColumns { get; set; }
    }
}
