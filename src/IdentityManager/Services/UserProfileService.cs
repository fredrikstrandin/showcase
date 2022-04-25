using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using IdentityManager.Interface;
using IdentityManager.Models;

namespace IdentityManager.Services
{
    public class UserProfileService : IProfileService
    {
        protected readonly ILogger Logger;
        protected readonly IUserStoreService _userStoreService;

        public UserProfileService(IUserStoreService userStoreService, ILogger<TestUserProfileService> logger)
        {
            _userStoreService = userStoreService;
            Logger = logger;
        }

        public virtual async  Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);
            if (context.RequestedClaimTypes.Any())
            {
                UserItem user = await _userStoreService.FindBySubjectIdAsync(context.Subject.GetSubjectId());
                if (user != null)
                {
                    context.AddRequestedClaims(user.Claims);
                }
            }

            context.LogIssuedClaims(Logger);
            return;
        }
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            Logger.LogDebug("IsActive called from: {caller}", context.Caller);
            
            context.IsActive = await _userStoreService.IsActiveAsync(context.Subject.GetSubjectId());
            
            return;
        }
    }
}
