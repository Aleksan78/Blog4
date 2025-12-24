using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Roles
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }
}
