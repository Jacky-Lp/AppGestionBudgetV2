using AppGestionBudget.DataBase.Configuration;
using AppGestionBudget.DataBase.DbEntities;
using Microsoft.EntityFrameworkCore;

public class GestionContext : DbContext
{
    public DbSet<ExpensesDb> Expenses { get; set; }
    public DbSet<UserDb> Users { get; set; }
    public string DbPath { get; }
    public GestionContext() {
        DbPath = System.IO.Path.Join("D:\\Dev\\C#\\Application", "bdd.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ExpensesConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}

