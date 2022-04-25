using IdentityManager.Interface;
using IdentityManager.Services;
using IdentityManager.Validator;

namespace IdentityManager.Extensions;

public static class IdentityServerBuilderExtensions
{
    public static IIdentityServerBuilder AddMongoDBUsers(this IIdentityServerBuilder builder)
    {
        builder.Services.AddSingleton<IUserStoreService, UserStoreService>();
        builder.AddProfileService<UserProfileService> ();
        builder.AddResourceOwnerValidator<MongoDBUserResourceOwnerPasswordValidator>();

        return builder;
    }
}