using Home_3.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_3.Data;

public class Home3Context : DbContext
{
    public Home3Context()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SPHome3DB2;Integrated Security=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId);

        modelBuilder.Entity<MyGroup>()
            .HasOne(g => g.Teacher)
            .WithMany(t => t.Groups)
            .HasForeignKey(g => g.TeacherId);

        modelBuilder.Entity<MyGroup>()
            .HasIndex(x => x.Name)
            .IsUnique();
    }

    public DbSet<MyGroup> Groups { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
}
