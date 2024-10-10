using BoilerPlate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Data.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<AuthUser> AuthUsers { get; set; }

    public DbSet<Client> Clients {  get; set; }  
}
