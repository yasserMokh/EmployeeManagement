using EmployeeManagement.Web.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManagement.Web.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();            
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(c => c.GetForeignKeys())){
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
