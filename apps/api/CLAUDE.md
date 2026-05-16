# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
dotnet run          # HTTP on port 5009
dotnet build
dotnet ef migrations add <Name>   # add EF Core migration
dotnet ef database update         # apply migrations to local SQL Server
```

No test project exists.

## Architecture

Single .NET 10 Minimal API project. No Controllers — all routes are defined as extension methods in `Endpoints/` and registered in `Program.cs` via `app.Map*Endpoints()`.

**Request flow:** `Program.cs` (service registration + endpoint mapping) → `Endpoints/*.cs` (grouped route handlers via `app.MapGroup`) → `AppDbContext` (EF Core, injected directly into handler delegates)

**Layers:**
- `Models/` — EF entities inheriting `BaseEntity` (adds `CreatedAt`/`UpdatedAt`) plus DTOs in `AuthModels.cs` and enums in `Common.cs`
- `Endpoints/` — one file per domain (Auth, User, Child, MeaslesScan, LocationLog, Todo); each exports a single extension method on `WebApplication`
- `Services/` — `ITokenService` / `TokenService` for JWT generation; BCrypt used for password hashing
- `AppDbContext.cs` — root-level; 6 DbSets (Users, Children, MeaslesScans, ScanPhotos, LocationLogs, Todos)

**Roles** (`Models/Common.cs`): `Admin = 0`, `HealthCareOfficer = 1`, `Parent = 2`

**Scan lifecycle** (`ScanStatus` enum): `Pending` → `AI_Confirmed` → `Officer_Verified` → `Cleared`

## Auth

JWT is configured via `appsettings.json` (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`). Tokens expire in 24 hours. Secure endpoints with `.RequireAuthorization()` or `.RequireAuthorization(p => p.RequireRole("Admin"))`.

## Coding Conventions

- Nullable reference types are enabled (`<Nullable>enable</Nullable>`) — use `string?` for optional strings
- Implicit usings are enabled — no need to import `System`, `System.Linq`, etc.
- All DB calls must be async (`ToListAsync`, `SaveChangesAsync`, `FindAsync`)
- Endpoint handlers receive `AppDbContext` and services as delegate parameters (constructor DI is not used in endpoints)

## Gotchas

- `appsettings.Development.json` is git-ignored; local overrides go there
- The database instance is `SHANTOTUF\SQLEXPRESS`, database name `MorboLens`, Windows auth — only works on the dev machine with that SQL Server instance
- `ScanPhoto.ImageData` stores raw binary (VARBINARY MAX) — large payloads go through EF; keep in mind for performance
- Forgot-password flow uses a hardcoded verification code (`"12345"`) — not production-ready
- CORS currently allows all origins/methods/headers
