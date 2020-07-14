using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaMark.Models;

namespace MediaMark.Models
{
    public class MediaMarkContext : DbContext
    {
        public MediaMarkContext (DbContextOptions<MediaMarkContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cart>()
            .HasOne<Products>(p => p.Products)
            .WithMany(p => p.ShoppingCart)
            .HasForeignKey(p => p.RefProductID);

            builder.Entity<Cart>()
            .HasOne<User>(p => p.User)
            .WithMany(p => p.ShoppingCart)
            .HasForeignKey(p => p.RefUserID);


            builder.Entity<OrderDetails>()
            .HasOne<Products>(p => p.Products)
            .WithMany(p => p.OrderDetail)
            .HasForeignKey(p => p.RefProductID);

            builder.Entity<OrderDetails>()
            .HasOne<Order>(p => p.Order)
            .WithMany(p => p.OrderDetail)
            .HasForeignKey(p => p.RefOrderID);

            
            builder.Entity<Products>()
            .HasOne<Category>(p => p.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.RefCateogryID);

            builder.Entity<Order>()
            .HasOne<User>(p => p.User)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.RefUserID);

        }

    }
}
