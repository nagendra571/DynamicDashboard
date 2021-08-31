using System;
using System.Collections.Generic;

namespace Dynamic_User_Defined_Dashboards.Models
{
    public partial class DashboardLinkedElements
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public int WidgetID { get; set; }
        public bool IsDefaultElement { get; set; }
        public string Placement { get; set; }
    }
}
