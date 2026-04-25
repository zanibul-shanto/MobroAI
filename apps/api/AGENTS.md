# MorboLensAI AGENTS.md

## Project Overview
Two-project solution:
- **Backend**: .NET 10 Minimal API (MorboLensAI.csproj) for AI-powered measles triage
- **Mobile**: .NET MAUI app (MorboMobile.csproj) for patient-facing frontend

Backend uses EF Core with SQL Server. Single-project structure for API.

## Developer Commands
- `dotnet run` - Runs backend API on HTTP port 5009 (see `Properties/launchSettings.json`)
- `dotnet build` - Builds both backend and mobile projects
- `dotnet ef migrations add <Name>` - Add new migration (requires `Microsoft.EntityFrameworkCore.Design`)
- `dotnet ef database update` - Apply migrations to SQL Server
- For mobile: Use Visual Studio with MAUI workloads or `dotnet build -t:Run` in MorboMobile/

## Architecture
- **Pattern**: Minimal APIs only. Do NOT create Controllers.
- **Endpoint registration**: Add endpoints via extension methods (e.g., `app.MapTodoEndpoints()` in `Program.cs`)
- **Models**: Located in `Models/` directory
- **DbContext**: `AppDbContext` in root - registers `DbSet<Todo> Todos`
- **Entry point**: `Program.cs` - all service registration and endpoint mapping visible here
- **Mobile**: Separate MAUI project targeting Android/iOS/macOS/Windows

## Database
- **Dev**: SQL Server (connection string in `appsettings.json` - local instance `SHANTOTUF\SQLEXPRESS`)
- **Testing**: In-memory provider available via `Microsoft.EntityFrameworkCore.InMemory`
- **Migrations**: Exist in `Migrations/` folder; initial migration creates `Todos` table
- **Important**: Connection string uses Windows auth - adjust for your environment

## Coding Conventions
- `<Nullable>enable</Nullable>` is active - use `string?` for nullable strings
- Always use async EF Core methods: `ToListAsync()`, `SaveChangesAsync()`, etc.
- Endpoint handlers receive `AppDbContext` via DI in the delegate parameters
- Group related endpoints with `app.MapGroup("/path")`
- JWT authentication configured - endpoints require `[Authorize]` or `.RequireAuthorization()`

## Gotchas
- **Transient data**: In-memory DB loses all data on app restart
- **Migrations**: Don't create migrations for in-memory testing; only for actual schema changes
- **appsettings.Development.json**: Excluded from git (contains local overrides)
- **No test project**: No `*Test*.csproj` exists yet - unit tests should use in-memory provider
- **Mobile platform**: Requires MAUI workloads installed (`dotnet workload install maui`)
- **JWT Key**: Hardcoded in appsettings.json - rotate for production
- **Roles**: Admin role required for DELETE operations on todos