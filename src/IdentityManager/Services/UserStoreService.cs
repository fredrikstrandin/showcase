using Duende.IdentityServer.Test;
using IdentityManager.Interface;
using IdentityManager.Models;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityManager.Services;

public class UserStoreService : IUserStoreService
{
    private readonly IUserStoreRepository _userStoreRepository;
    private readonly IPasswordService _passwordService;

    public UserStoreService(IUserStoreRepository userStoreRepository, IPasswordService passwordService)
    {
        _userStoreRepository = userStoreRepository;
        _passwordService = passwordService;
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var ps = await _userStoreRepository.FindHashPasswordAndSaltByUsernameAsync(username);
        
        if(ps.Salt == null || ps.Hash == null)
        {
            return false;
        }

        return _passwordService.CompareHash(password, ps.Hash, ps.Salt);
    }

    public async Task<UserItem> FindBySubjectIdAsync(string subjectId)
    {
        return await _userStoreRepository.FindBySubjectIdAsync(subjectId);
    }

    public async Task<UserItem> FindByUserNameAsync(string username)
    {
        return await _userStoreRepository.FindByUsernameAsync(username);
    }

    public async Task<UserItem> FindByExternalProviderAsync(string provider, string userId)
    {
        return await _userStoreRepository.FindByExternalProviderAsync(provider, userId);
    }

    public async Task<UserItem> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims)
    {
        // create a list of claims that we want to transfer into our store
        var filtered = new List<Claim>();

        foreach (var claim in claims)
        {
            // if the external system sends a display name - translate that to the standard OIDC name claim
            if (claim.Type == ClaimTypes.Name)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
            }
            // if the JWT handler has an outbound mapping to an OIDC claim use that
            else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
            {
                filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
            }
            // copy the claim as-is
            else
            {
                filtered.Add(claim);
            }
        }

        // if no display name was provided, try to construct by first and/or last name
        if (!filtered.Any(x => x.Type == JwtClaimTypes.Name))
        {
            var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
            var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
            if (first != null && last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }


        // check if a display name is available, otherwise fallback to subject id
        var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? "";

        // create new user
        UserItem user = await _userStoreRepository.AddProvisionUserAsync(name, provider, userId, claims);

        return user;
    }

    public Task<bool> IsActiveAsync(string subjectId)
    {
        return _userStoreRepository.IsActive(subjectId);
    }

    public Task<string> AddUserAsync(string nickname, string password, byte[] salt, string sub, List<Claim> claims)
    {
        return _userStoreRepository.CreateUserAsync(nickname, password, salt, sub, claims);
    }
}
