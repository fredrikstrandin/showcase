using CustomerManager.Model;
using GraphQL;
using GraphQL.Types;
using Northstar.Message;
using NorthStarGraphQL.GraphQL.Types;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;

namespace NorthtarGraphQL.GraphQL;

public class NorthStarMutation : ObjectGraphType
{
    readonly private ICustomerRepository _customerRepository;
    readonly private IIdentityService _identityService;

    public NorthStarMutation(ICustomerRepository customerRepository, IIdentityService identityService)
    {
        _customerRepository = customerRepository;
        _identityService = identityService;

        FieldAsync<CustomerGraphType>(
            "createCustomer",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "nickName" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "firstName" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "lastName" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "street" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "zip" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "city" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "monthlyIncome" }
            ),
            resolve: async context =>
            {
                LoginCreateItem loginItem = new LoginCreateItem(
                        context.GetArgument<string>("nickName"),
                        context.GetArgument<string>("email"),
                        context.GetArgument<string>("password"),
                        new List<ClaimItem>() { new ClaimItem("userType", "member") });

                var login = await _identityService.CreateLoginAsync(loginItem);

                if(login.error == null)
                {
                    CustomerCreateItem customerItem = new CustomerCreateItem(
                        login.id,
                        context.GetArgument<string>("firstName"),
                        context.GetArgument<string>("lastName"),
                        context.GetArgument<string>("email"),
                        context.GetArgument<string>("street"),
                        context.GetArgument<string>("zip"),
                        context.GetArgument<string>("city"),
                        context.GetArgument<int>("monthlyIncome"));

                    var ret = await _customerRepository.CreateAsync(customerItem, context.CancellationToken);

                    if (string.IsNullOrWhiteSpace(ret.error))
                    {
                        if (ret.id != null)
                        {
                            return new CustomerItem(ret.id,
                                customerItem.Id,
                                customerItem.FirstName,
                                customerItem.LastName,
                                customerItem.Email,
                                customerItem.Street,
                                customerItem.Zip,
                                customerItem.City,
                                customerItem?.MonthlyIncome ?? 0);
                        }
                    }
                    else
                    {
                        context.Errors.Add(new ExecutionError(ret.error));
                    }
                }

                return null;
            }
        );
    }
}