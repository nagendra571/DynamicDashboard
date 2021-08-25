using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class BusinessRoleDashboardMapping
    {
        public int ID { get; set; }
        public int RoleId { get; set; }
        public int DashboardId { get; set; }

        [NotMapped]
        public string SelectedDashboardIds{ get; set; }
    }
}
