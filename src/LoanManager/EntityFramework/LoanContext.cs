using LoanManager.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.EntityFramework
{
    public class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions<LoanContext> options) : base(options)
        {

        }

        public DbSet<LoanApplicationEntity> LoanApplications { get; set; }

        public DbSet<RejectedEntity> RejectedLoanApplications { get; set; }
        public DbSet<DecisionEntity> Decisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
