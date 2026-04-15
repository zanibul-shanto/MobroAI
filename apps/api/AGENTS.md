# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Non-Obvious Project Information

- **Stack**: .NET 10 Minimal API with Entity Framework Core (In-Memory).
- **Core Entry**: `Program.cs` contains all route mappings; there are no separate Controller classes.
- **Data Persistence**: Uses `Microsoft.EntityFrameworkCore.InMemory`. Data is lost on application restart.
- **Code Style**:
  - Uses File-Scoped Namespaces (e.g., `namespace MorboLensAI;`).
  - Minimal API pattern: Routes are defined directly on `app` in `Program.cs`.
- **Directory Usage**:
  - `Models/` and `Repository/` are defined in `.csproj` but currently empty. New models should be placed in `Models/`.
- **Commands**:
  - Build: `dotnet build`
  - Run: `dotnet run`
  - Watch: `dotnet watch`
