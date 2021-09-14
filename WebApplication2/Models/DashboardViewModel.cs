using Dynamic_User_Defined_Dashboards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SelectedElements { get; set; }
        public string DefaultedElements { get; set; }


        public List<DashboardLinkedElements> SelectedElementsWithOrder { get; set; }
        public List<DashboardLinkedElements> DefaultedElementsWithOrder { get; set; }
    }
}
