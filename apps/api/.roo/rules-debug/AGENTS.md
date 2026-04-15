# Project Debug Rules (Non-Obvious Only)

- **In-Memory DB**: Check `Program.cs` for `builder.Services.AddDatabaseDeveloperPageExceptionFilter()`.
- **Launch Settings**: Check `Properties/launchSettings.json` for environment-specific configurations.
- **Transient Data**: If data disappears, it's likely due to an application restart (In-Memory DB).
