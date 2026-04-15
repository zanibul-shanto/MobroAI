using System;
using Microsoft.EntityFrameworkCore;

namespace MorboLensAI;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}