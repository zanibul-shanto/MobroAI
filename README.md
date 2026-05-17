# MobroAI Monorepo

Full-stack AI-powered child health monitoring platform.

## Apps

| App | Stack | Path |
|-----|-------|------|
| Mobile | React Native (Expo) | [`apps/mobile`](./apps/mobile) |
| API | ASP.NET Core 10 Minimal API | [`apps/api`](./apps/api) |

## Quick Start

### Mobile (Expo)
```sh
cd apps/mobile
npm install
npx expo start
```

### API (.NET)
```sh
cd apps/api
dotnet run
# Runs on http://localhost:5009
```

## Docs

- [Architecture](./docs/architecture.md)
- [API Reference](./docs/api-reference.md)
- [Dev Setup](./docs/setup.md)

## Structure

```
MobroAI/
├── apps/
│   ├── mobile/     # Expo app (React Native, Zustand, Expo Router)
│   └── api/        # .NET 10 Minimal API (EF Core, JWT, SQL Server)
├── packages/       # Reserved for future shared code
├── docs/           # Project-wide documentation
└── .github/
    └── workflows/  # CI for each app (path-filtered)
```
