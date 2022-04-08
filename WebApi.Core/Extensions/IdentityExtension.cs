using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Extensions
{
    public static class IdentityExtension
    {
        public static string GetEmail(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).Claims.FirstOrDefault(c => c.Type == "email").Value;
        }
    }
}
