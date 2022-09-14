using System.ComponentModel.DataAnnotations;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }
        [StringLength(30)]
        public string ManagerName { get; set; }
        [StringLength(25)]
        public string ManagerSurname { get; set; }
        [StringLength(30)]
        public string ManagerUserName { get; set; }
        [StringLength(50)]
        public string ManagerEmail { get; set; }
        [StringLength(10)]
        public string ManagerPhone { get; set; }
        public DateTime ManagerCreatedDate { get; set; }
    }
}
