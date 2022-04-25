using Duende.IdentityServer.Validation;
using IdentityManager.Interface;
using IdentityModel;

namespace IdentityManager.Validator;

public class MongoDBUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly IUserStoreService _users;

    public MongoDBUserResourceOwnerPasswordValidator(IUserStoreService users)
    {
        _users = users;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        if (await _users.ValidateCredentialsAsync(context.UserName, context.Password))
        {
            var user = await _users.FindByUserNameAsync(context.UserName);
            context.Result = new GrantValidationResult(user.SubjectId.ToString(), OidcConstants.AuthenticationMethods.Password, user.Claims);
        }

        return;
    }
}
