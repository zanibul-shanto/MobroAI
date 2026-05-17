# MobroAI Monorepo — Claude Code Guide

## Layout

```
apps/mobile/   React Native (Expo) — see apps/mobile/CLAUDE.md
apps/api/      ASP.NET Core 10 — see apps/api/CLAUDE.md
packages/      reserved for future shared TS/C# code
docs/          architecture, API reference, setup guide
```

## Working in each app

Always `cd` into the app directory before running commands:

```sh
# Mobile
cd apps/mobile && npx expo start

# API
cd apps/api && dotnet run
```

## Key decisions

- CI is path-filtered: pushing to `apps/mobile/**` only triggers mobile-ci, etc.
- Each app keeps its own `.gitignore` — root `.gitignore` covers OS/IDE/secrets only.
- `apps/api/appsettings.Development.json` is gitignored — create it locally with real connection strings and JWT key.
- `apps/mobile/.env.local` is gitignored — set `EXPO_PUBLIC_API_URL` there for local dev.

## Refer to app-level guides

- Mobile details: `apps/mobile/CLAUDE.md`
- API details: `apps/api/CLAUDE.md`
