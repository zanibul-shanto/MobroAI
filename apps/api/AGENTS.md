# MobroLens AGENTS.md

## Project Overview
Single .NET 10 Minimal API project (MobroLens.csproj) for AI-powered measles triage. No mobile project exists despite README mentions.

## Developer Commands
- `dotnet run` - Runs on HTTP port 5009 (see `Properties/launchSettings.json`)
- `dotnet build` - Builds the project
- `dotnet ef migrations add <Name>` - Add migration (Design package already referenced)
- `dotnet ef database update` - Apply migrations to SQL Server

## Architecture
- **Pattern**: Minimal APIs only. No Controllers.
- **Entry point**: `Program.cs` - service registration and endpoint mapping
- **Endpoints**: Extension methods in `Endpoints/` directory, mapped in `Program.cs` via `app.MapTodoEndpoints()` etc.
- **Models**: `Models/` directory (Todo, User, AuthModels, Common with Role enum)
- **Services**: `Services/` directory (ITokenService, TokenService using BCrypt.Net-Next)
- **DbContext**: `AppDbContext` in root - registers `Todos` and `Users` DbSets

## Database
- **Dev**: SQL Server `SHANTOTUF\SQLEXPRESS`, database `MorboLens` (Windows auth, `appsettings.json`)
- **Migrations**: `Migrations/` folder with multiple schema changes (Users table added post-initial)
- **Note**: Connection string uses `TrustServerCertificate=True`

## Coding Conventions
- `<Nullable>enable</Nullable>` - use `string?` for nullable strings
- `<ImplicitUsings>enable</ImplicitUsings>` is active
- Async EF Core methods: `ToListAsync()`, `SaveChangesAsync()`, etc.
- Endpoint handlers receive `AppDbContext` and other services via DI in delegate parameters
- Group endpoints with `app.MapGroup("/path")`

## Auth
- JWT configured via `appsettings.json` (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`)
- Access token expires in 1440 minutes (24h)
- Endpoints require `[Authorize]` or `.RequireAuthorization()`
- Admin role required for DELETE on `/todoitems/{id}`
- Roles: `Admin = 0`, `User = 1`, `HealthCareOfficer = 2` (see `Models/Common.cs`)

## Gotchas
- **No test project**: No `*Test*.csproj` exists
- **appsettings.Development.json**: Excluded from git
- **JWT Key**: Hardcoded in `appsettings.json` - rotate for production
- **In-memory provider**: `Microsoft.EntityFrameworkCore.InMemory` not currently referenced; only SQL Server configured
