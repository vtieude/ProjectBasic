using Service.Login.BAL.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Login.BAL
{
    public interface ILoginService
    {
        Task<string> Authenticate(string username, string password);
        Task<string> Register(string email, string password);
        Task<LoginUserInforDto> GetLoginUserInfo();
        Task ForgotPassword(string email, string callbackUrl);
    }
}
