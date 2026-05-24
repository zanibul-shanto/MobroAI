# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Monorepo Layout

```
apps/mobile/   React Native (Expo 54) — see apps/mobile/CLAUDE.md
apps/api/      ASP.NET Core 10 Minimal API — see apps/api/CLAUDE.md
ML_Model/      Keras/ONNX measles detection model + conversion script
docs/          Architecture, API reference, setup guide
packages/      Reserved for future shared TS/C# code
```

## Commands

Always `cd` into the app directory before running commands:

```sh
# Mobile (from apps/mobile/)
npx expo start              # dev server
npx expo start --lan --clear

# API (from apps/api/)
dotnet run                  # HTTP on port 5009
dotnet ef migrations add <Name>
dotnet ef database update

# ML Model conversion (from repo root)
python3 ML_Model/convert_to_onnx.py --input ML_Model/model.h5
python3 ML_Model/convert_to_onnx.py --input ML_Model/model.h5 --output ML_Model/model.onnx
```

## End-to-End Architecture

MobroAI is a child health monitoring platform. The full request path is:

**Mobile (Expo/React Native)** → HTTP (JWT Bearer) → **API (.NET 10)** → SQL Server (`MorboLens` db)

For measles scans specifically:
**Mobile camera/gallery** → image upload → **API** stores binary in `ScanPhoto.ImageData` → AI model runs → status updates through `Pending → AI_Confirmed → Officer_Verified → Cleared`

The ONNX model (`ML_Model/monkeynet_lite.onnx`) is a MobileNetV2-based classifier (input: `224×224×3`, output: 4 classes). The `.h5` source is Keras 3 format — use `convert_to_onnx.py` to regenerate ONNX if the model is retrained. Required Python packages: `keras`, `torch`, `onnx`, `onnxscript`.

## Key Cross-App Decisions

- CI is path-filtered: `apps/mobile/**` triggers mobile-ci only, etc.
- Each app has its own `.gitignore`; root `.gitignore` covers OS/IDE/secrets.
- `apps/api/appsettings.Development.json` is gitignored — create locally with connection string and JWT key.
- `apps/mobile/.env.local` is gitignored — set `EXPO_PUBLIC_API_URL` for local dev.
- The mobile API client (`api/api.ts`) injects JWT automatically via an Axios interceptor reading from SecureStore.

## App-Level Guides

Detailed commands, architecture, and gotchas are in the app-level files — read these before touching either app:

- **Mobile:** `apps/mobile/CLAUDE.md`
- **API:** `apps/api/CLAUDE.md`

## Coding Behavioral Guidelines

Four principles to follow on every task:

1. **Think before coding** — Surface assumptions and tradeoffs upfront. Don't silently choose between interpretations; ask when ambiguous.
2. **Simplicity first** — Minimum code that solves the problem. Nothing speculative, no unnecessary abstractions.
3. **Surgical changes** — Every changed line must trace directly to the user's request. No scope creep into unrelated improvements.
4. **Goal-driven execution** — Define verifiable success criteria before starting. Loop until verified.

## graphify

This project has a knowledge graph at `graphify-out/` with god nodes, community structure, and cross-file relationships.

- ALWAYS read `graphify-out/GRAPH_REPORT.md` before reading source files, running grep/glob, or answering codebase questions.
- If `graphify-out/wiki/index.md` exists, navigate it instead of reading raw files.
- For cross-module questions, prefer `graphify query "<question>"`, `graphify path "<A>" "<B>"`, or `graphify explain "<concept>"` over grep.
- After modifying code, run `graphify update .` to keep the graph current (AST-only, no API cost).
