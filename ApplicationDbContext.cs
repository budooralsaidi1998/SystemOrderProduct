using Microsoft.EntityFrameworkCore;
using SystemProductOrder.models;
using System;


namespace SystemProductOrder
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> products { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderPorduct> orderPorducts { get; set; }
        public DbSet<Review> reviews { get; set; }

    }
}

