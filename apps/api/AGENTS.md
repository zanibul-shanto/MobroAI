# MorboLensAI AGENTS.md

## Project Overview
.NET 10 Minimal API project for AI-powered measles triage. Single-project structure using EF Core with SQL Server.

## Developer Commands
- `dotnet run` - Runs on HTTP port 5009 (see `Properties/launchSettings.json`)
- `dotnet build` - Build the project
- `dotnet ef migrations add <Name>` - Add new migration (requires `Microsoft.EntityFrameworkCore.Design` package)
- `dotnet ef database update` - Apply migrations

## Architecture
- **Pattern**: Minimal APIs only. Do NOT create Controllers.
- **Endpoint registration**: Add endpoints via extension methods (e.g., `app.MapTodoEndpoints()` in `Program.cs`)
- **Models**: Located in `Models/` directory
- **DbContext**: `AppDbContext` in root - registers `DbSet<Todo> Todos`
- **Entry point**: `Program.cs` - all service registration and endpoint mapping visible here

## Database
- **Dev**: SQL Server (connection string in `appsettings.json` - local instance `SHANTOTUF\SQLEXPRESS`)
- **Testing**: In-memory provider available via `Microsoft.EntityFrameworkCore.InMemory`
- **Migrations**: Exist in `Migrations/` folder; initial migration creates `Todos` table

## Coding Conventions
- `<Nullable>enable</Nullable>` is active - use `string?` for nullable strings
- Always use async EF Core methods: `ToListAsync()`, `SaveChangesAsync()`, etc.
- Endpoint handlers receive `AppDbContext` via DI in the delegate parameters
- Group related endpoints with `app.MapGroup("/path")`

## Gotchas
- **Transient data**: In-memory DB loses all data on app restart
- **Migrations**: Don't create migrations for in-memory testing; only for actual schema changes
- **appsettings.Development.json**: Excluded from git (contains local overrides)
- **No test project**: No `*Test*.csproj` exists yet - unit tests should use in-memory provider
