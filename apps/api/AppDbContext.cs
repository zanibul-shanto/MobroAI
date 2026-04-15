using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;

namespace MorboLensAI;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<User> Users => Set<User>();
}
