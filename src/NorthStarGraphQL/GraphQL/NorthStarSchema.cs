using GraphQL.Types;

namespace NorthtarGraphQL.GraphQL;
public class NorthStarSchema : Schema
{
    public NorthStarSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<NorthStarQuery>();
        Mutation = serviceProvider.GetRequiredService<NorthStarMutation>();
    }
}
