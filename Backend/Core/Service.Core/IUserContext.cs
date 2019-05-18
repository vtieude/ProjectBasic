using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IUserContext
    {
        UserLogin CurrentUser { get; }
    }
    public class UserLogin
    {
        public long SiteId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
    public class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserLogin CurrentUser
        {
            get
            {
                var user = new UserLogin();
                Claim usernameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                Claim idClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Sid");
                user.UserName = usernameClaim == null ? "" : usernameClaim.Value;

                user.UserId = idClaim?.Value ?? "";

                return user;
            }
        }
    }
}
