using System;
using System.Collections.Generic;

namespace Service.Core.DAL.Models
{
    public partial class AspNetUserClaims
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public DateTimeOffset? SysChgDate { get; set; }
        public string SysChgLogin { get; set; }
        public string SysChgType { get; set; }
        public int? SysChgCnt { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
