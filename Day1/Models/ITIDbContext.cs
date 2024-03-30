using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Day1.Models
{
    public class ITIDbContext:IdentityDbContext<ApplicationUser>
    {
        public ITIDbContext()
        {
            
        }
        public ITIDbContext(DbContextOptions options):base(options) 
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
