using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SamayaExam.Models;

namespace SamayaExam.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
      
    }
}
