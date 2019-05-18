using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Login.BAL.Model
{
    public class AspNetUserInput : IdentityUser
    {
        public int SysChgCnt { get; set; } = 1;
        public DateTimeOffset SysChgDate { get; set; } = DateTimeOffset.Now;
        public string SysChgLogin { get; set; } = "1";
        public string SysChgType { get; set; } = "Y";
    }
}
