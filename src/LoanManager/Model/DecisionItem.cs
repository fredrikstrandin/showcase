using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class DecisionItem
    {
        public string LoanId { get; set; }
        public DateTime Created { get; set; }
        public bool Aproved { get; set; }
        public List<string> Decisions { get; set; }
    }
}
