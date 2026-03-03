
using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CyberQuiz.API.Controllers
{


    public class UserManagementController
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserManagementController(SignInManager<ApplicationUser> SignInManager)
        {
            signInManager = SignInManager;
        }
    }
}
