using CustomerManager.Model;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarQuery 
{
        public UserItem GetUser() => 
                new UserItem("1", "Lars", "Larsson", "fre@svt.se", "sveav 34", "112 11", "Stockholm", 34000);
    
}