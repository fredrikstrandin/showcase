﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class DecisionEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LoanId { get; set; }
        public DateTime Created { get; set; }
        public bool Aproved { get; set; }
        public string Decision { get; set; }
    }
}
