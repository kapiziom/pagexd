using Microsoft.EntityFrameworkCore;
using pagexd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Data
{
    public class ModelDbContext:DbContext
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options)
            : base(options) { }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Post>()
                .HasOne(a => a.Photo)
                .WithOne(b => b.Post)
                .HasForeignKey<Photo>(b => b.PostIDref);
        }

    }
}
