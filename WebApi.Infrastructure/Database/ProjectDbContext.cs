using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Domain;
using WebApi.Domain.Identity;

namespace WebApi.Infrastructure.Database
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseNpgsql(@"Host=192.168.0.221;Username=postgres;Password=password;Database=postgres");
    }
}
