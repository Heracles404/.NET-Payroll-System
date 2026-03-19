using Microsoft.EntityFrameworkCore;
using NET_Payroll_System.Models;

namespace NET_Payroll_System.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .Property(e => e.DateOfBirth)
            .HasColumnType("date");
    }
    
    public DbSet<Employee> Employees => Set<Employee>();
}
