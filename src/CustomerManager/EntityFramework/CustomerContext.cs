using CustomerManager.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.EntityFramework
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }
        public DbSet<CustomerEntity> Customers { get; set; }
    }
}
