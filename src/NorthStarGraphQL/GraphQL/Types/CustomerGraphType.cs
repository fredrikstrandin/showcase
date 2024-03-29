﻿using CustomerManager.Model;
using GraphQL.Types;

namespace NorthStarGraphQL.GraphQL.Types
{
    public class CustomerGraphType : ObjectGraphType<CustomerItem>
    {
        public CustomerGraphType()
        {
            Field(x => x.Id);
            Field(x => x.PersonalNumber);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Email);
            Field(x => x.Street);
            Field(x => x.City);
            Field(x => x.Zip);
            Field(x => x.MonthlyIncome, nullable: true);
        }
    }
}
