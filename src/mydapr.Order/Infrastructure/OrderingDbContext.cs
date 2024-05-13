using Link.Mydapr.Service.Ordering.Models;
using Microsoft.EntityFrameworkCore;

namespace Link.Mydapr.Service.Ordering.Infrastructure
{
    public class OrderingDbContext : DbContext
    {
        public DbSet<Order> Orders  {get;set;}

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public OrderingDbContext(){}

        public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
