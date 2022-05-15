using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WebApi.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    public string UserId { get; }
    public List<KeyValuePair<string, string>> Claims { get; set; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable()
                     .Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList() ??
                 throw new InvalidOperationException("Claims not found");
        
        if (Claims?.Find(x => x.Key.Equals("user_id")).Value != null)
            UserId = Claims.Find(x => x.Key.Equals("user_id")).Value;
    }
}