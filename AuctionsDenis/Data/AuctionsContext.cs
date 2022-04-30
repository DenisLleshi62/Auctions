using AuctionsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionsProject.Data;

public class AuctionsContext : DbContext
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Bids> Bids { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AuctionsDB;Integrated Security=True");
    }
}