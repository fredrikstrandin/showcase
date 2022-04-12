using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManager.Model
{
    public class LoanRequestRespons
    {
        public LoanRequestRespons(string id, bool isSuccess)
        {
            Id = id;
            IsSuccess = isSuccess;
        }

        public string Id { get; }
        public bool IsSuccess { get; }
    }
}