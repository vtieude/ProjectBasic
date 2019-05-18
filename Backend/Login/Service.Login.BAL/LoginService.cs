using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Service.Core.DAL;
using Service.Core.DAL.Models;
using Service.Login.BAL.Dto;
using Service.Login.BAL.Model;

namespace Service.Login.BAL
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<AspNetUsers> _aspNetUsersRepository;
        private readonly SignInManager<AspNetUserInput> _signInManager;
        private readonly UserManager<AspNetUserInput> _userManager;
        public LoginService(SignInManager<AspNetUserInput> signInManager, IRepository<AspNetUsers> aspNetUsersRepository,
            UserManager<AspNetUserInput> userManager)
        {
            _aspNetUsersRepository = aspNetUsersRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<string> Authenticate(string username, string password)
        {
            IdentityUser user = await _signInManager.UserManager.FindByEmailAsync(username);
            if (user == null)
            {
                return null;
            }
            return "ok";
        }

        public Task ForgotPassword(string email, string callbackUrl)
        {
            throw new NotImplementedException();
        }

        public Task<LoginUserInforDto> GetLoginUserInfo()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(string email, string password)
        {
            var user = new AspNetUserInput { UserName = email, Email = email };
            var result = new IdentityResult();
            try
            {
                result = await _userManager.CreateAsync(user, password);
                return "success";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
