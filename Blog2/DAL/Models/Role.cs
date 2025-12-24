using Microsoft.AspNetCore.Identity;

namespace Blog2.DAL.Models
{
    public class Role : IdentityRole
    {        
        public int? SecurityLvl { get; set; } = null;
    }
}
