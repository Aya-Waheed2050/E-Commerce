using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models.Roles
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(256, ErrorMessage = "Role Name Cant not Exceed 256 Characters")]
        public string Name { get; set; }
    }
}
