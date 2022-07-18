using NorthStarGraphQL.GraphQL.Types.User;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarQuery 
{
        public UserType GetUser() => 
                new UserType("1", "Lars", "Larsson", "fre@svt.se", "sveav 34", "112 11", "Stockholm");
    
}