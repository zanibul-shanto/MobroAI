# Project Documentation Rules (Non-Obvious Only)

- **Architecture**: This is a Minimal API project. The entire API surface area is visible in `Program.cs`.
- **Models**: Business logic entities are located in the project root (e.g., `Todo.cs`) but the intent is to move them to `Models/` as the project grows.
- **Dependencies**: View `MorboLensAI.csproj` for the latest .NET and EF Core package versions.
