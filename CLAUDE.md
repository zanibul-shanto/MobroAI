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

## graphify

This project has a knowledge graph at graphify-out/ with god nodes, community structure, and cross-file relationships.

Rules:
- ALWAYS read graphify-out/GRAPH_REPORT.md before reading any source files, running grep/glob searches, or answering codebase questions. The graph is your primary map of the codebase.
- IF graphify-out/wiki/index.md EXISTS, navigate it instead of reading raw files
- For cross-module "how does X relate to Y" questions, prefer `graphify query "<question>"`, `graphify path "<A>" "<B>"`, or `graphify explain "<concept>"` over grep — these traverse the graph's EXTRACTED + INFERRED edges instead of scanning files
- After modifying code, run `graphify update .` to keep the graph current (AST-only, no API cost).
