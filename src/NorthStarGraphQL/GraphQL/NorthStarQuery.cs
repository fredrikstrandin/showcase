using CustomerManager.Model;
using GraphQL.Types;
using NorthStarGraphQL.GraphQL.Types;
using NorthStarGraphQL.Models;

namespace NorthtarGraphQL.GraphQL;

public class NorthStarQuery : ObjectGraphType
{
    public NorthStarQuery()
    {
        Field<ListGraphType<CustomerGraphType>>(
            "customer", 
            resolve: context => new List<CustomerItem> {
                new CustomerItem("1", "7210210535", "Lars", "Larsson", "fre@svt.se", "sveav 34", "112 11", "Stockholm", 34000)
            }
        );
    }
}