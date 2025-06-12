using System.ComponentModel.DataAnnotations;
using AdminDashboard.Models.Roles;

namespace AdminDashboard.Models.Users
{
    public class UserRoleViewModel
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }

    }
}
