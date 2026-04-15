# Project Coding Rules (Non-Obvious Only)

- **Minimal APIs**: Do NOT create Controllers. Add all new endpoints to `Program.cs` using `app.Map[Method]`.
- **Database Context**: `TodoDb` is injected into endpoint delegates. Always use `async` versions of EF Core methods (e.g., `ToListAsync()`, `SaveChangesAsync()`).
- **Nullability**: `<Nullable>enable</Nullable>` is active. Use `string?` for optional properties like in `Todo.cs`.
- **Memory Storage**: Since the DB is in-memory, do not write migrations or look for a SQL connection string.
