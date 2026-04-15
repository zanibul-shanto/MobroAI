using Microsoft.EntityFrameworkCore;
using MorboLensAI.Models;

namespace MorboLensAI.Repository;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
