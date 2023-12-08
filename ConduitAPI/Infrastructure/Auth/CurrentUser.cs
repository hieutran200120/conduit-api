using System.Security.Claims;

namespace ConduitAPI.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public Guid? Id => _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier) != null ? new Guid(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : null;
    }
}
