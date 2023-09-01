using System.ComponentModel.DataAnnotations;

namespace BikeLostAndFound.ViewModels
{
    public class RoleManagerViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        
    }
}
