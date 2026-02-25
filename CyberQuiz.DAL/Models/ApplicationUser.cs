using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    // ApplicationUser ärver från IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public ICollection<UserResult> UserResults { get; set; }
    }
}
