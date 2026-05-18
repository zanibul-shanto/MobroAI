# Graph Report - MobroAI  (2026-05-19)

## Corpus Check
- 64 files · ~32,483 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 481 nodes · 573 edges · 48 communities (29 shown, 19 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 26 edges (avg confidence: 0.9)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `eed5fee0`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]
- [[_COMMUNITY_Community 6|Community 6]]
- [[_COMMUNITY_Community 7|Community 7]]
- [[_COMMUNITY_Community 8|Community 8]]
- [[_COMMUNITY_Community 9|Community 9]]
- [[_COMMUNITY_Community 10|Community 10]]
- [[_COMMUNITY_Community 11|Community 11]]
- [[_COMMUNITY_Community 12|Community 12]]
- [[_COMMUNITY_Community 13|Community 13]]
- [[_COMMUNITY_Community 14|Community 14]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]
- [[_COMMUNITY_Community 20|Community 20]]
- [[_COMMUNITY_Community 21|Community 21]]
- [[_COMMUNITY_Community 22|Community 22]]
- [[_COMMUNITY_Community 23|Community 23]]
- [[_COMMUNITY_Community 24|Community 24]]
- [[_COMMUNITY_Community 25|Community 25]]
- [[_COMMUNITY_Community 26|Community 26]]
- [[_COMMUNITY_Community 27|Community 27]]
- [[_COMMUNITY_Community 28|Community 28]]
- [[_COMMUNITY_Community 31|Community 31]]
- [[_COMMUNITY_Community 32|Community 32]]
- [[_COMMUNITY_Community 35|Community 35]]
- [[_COMMUNITY_Community 36|Community 36]]
- [[_COMMUNITY_Community 37|Community 37]]
- [[_COMMUNITY_Community 38|Community 38]]
- [[_COMMUNITY_Community 39|Community 39]]
- [[_COMMUNITY_Community 40|Community 40]]
- [[_COMMUNITY_Community 41|Community 41]]
- [[_COMMUNITY_Community 42|Community 42]]
- [[_COMMUNITY_Community 43|Community 43]]
- [[_COMMUNITY_Community 44|Community 44]]
- [[_COMMUNITY_Community 45|Community 45]]

## God Nodes (most connected - your core abstractions)
1. `dependencies` - 33 edges
2. `expo` - 14 edges
3. `MobroLens API Claude Guide` - 14 edges
4. `useAuthStore` - 13 edges
5. `Mobile App Claude Guide` - 12 edges
6. `MeaslesScanEndpoints` - 11 edges
7. `Colors` - 11 edges
8. `API (ASP.NET Core 10)` - 11 edges
9. `MobroAI` - 9 edges
10. `ChildEndpoints` - 8 edges

## Surprising Connections (you probably didn't know these)
- `MobroAI Monorepo README` --references--> `App Icon (Blue chevron/arrow logo on light blue background with design grid)`  [INFERRED]
  README.md → apps/mobile/assets/images/icon.png
- `API CI Workflow` --references--> `MobroLens API Claude Guide`  [INFERRED]
  .github/workflows/api-ci.yml → apps/api/CLAUDE.md
- `Mobile CI Workflow` --references--> `Mobile App Claude Guide`  [INFERRED]
  .github/workflows/mobile-ci.yml → apps/mobile/CLAUDE.md
- `MobroLens API Claude Guide` --references--> `Dev Environment Setup Guide`  [INFERRED]
  apps/api/CLAUDE.md → docs/setup.md
- `Mobile App AGENTS.md` --semantically_similar_to--> `Mobile App Claude Guide`  [INFERRED] [semantically similar]
  apps/mobile/AGENTS.md → apps/mobile/CLAUDE.md

## Hyperedges (group relationships)
- **MobroAI Platform Technology Stack** — readme_mobile_app, readme_api, readme_jwt_auth, readme_ef_core, readme_sql_server, readme_expo_router, readme_zustand [EXTRACTED 1.00]
- **MobroAI Database Entities** — readme_user_entity, readme_child_entity, readme_measles_scan_entity, readme_scan_photo_entity, readme_location_log_entity [EXTRACTED 1.00]
- **API Request Flow** — readme_program_cs, readme_endpoints, readme_appdbcontext [EXTRACTED 1.00]

## Communities (48 total, 19 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (37): api, LoginForm, loginSchema, LoginScreen(), styles, RegisterForm, registerSchema, ROLES (+29 more)

### Community 1 - "Community 1"
Cohesion: 0.09
Nodes (39): MobroLens API AGENTS.md, MobroLens API Claude Guide, MorboLens API README, MobroAI Monorepo Claude Guide, AI-Powered Measles Triage, Zustand Auth Store (expo-secure-store backed), Axios Request Interceptor (auto-JWT injection), Background Location Tracking Service (+31 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (33): dependencies, axios, expo, expo-constants, expo-font, expo-haptics, expo-image-picker, expo-linking (+25 more)

### Community 3 - "Community 3"
Cohesion: 0.09
Nodes (30): API (ASP.NET Core 10), AppDbContext, Auth Endpoints (/auth), Axios, BCrypt, CHILD Entity, Children Endpoints (/children), EF Core (+22 more)

### Community 4 - "Community 4"
Cohesion: 0.07
Nodes (27): backgroundColor, backgroundImage, foregroundImage, monochromeImage, adaptiveIcon, edgeToEdgeEnabled, permissions, predictiveBackGestureEnabled (+19 more)

### Community 5 - "Community 5"
Cohesion: 0.12
Nodes (15): LocationData, saveLocation(), InitialLayout(), unstable_settings, Config, Language, TranslationKeys, translations (+7 more)

### Community 6 - "Community 6"
Cohesion: 0.29
Nodes (7): scripts, android, ios, lint, reset-project, start, web

### Community 7 - "Community 7"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 8 - "Community 8"
Cohesion: 0.15
Nodes (12): AllowedHosts, ConnectionStrings, DefaultConnection, Jwt, AccessTokenExpirationMinutes, Audience, Issuer, Key (+4 more)

### Community 9 - "Community 9"
Cohesion: 0.15
Nodes (7): BaseEntity, Child, LocationLog, MeaslesScan, ScanPhoto, Todo, User

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (26): Architecture, Auth, code:bash (dotnet run          # HTTP on port 5009), Coding Conventions, Commands, Gotchas, API App (ASP.NET Core 10), Mobile App (React Native / Expo) (+18 more)

### Community 12 - "Community 12"
Cohesion: 0.28
Nodes (5): HapticTab(), IconMapping, IconSymbol(), IconSymbolName, MAPPING

### Community 16 - "Community 16"
Cohesion: 0.22
Nodes (7): exampleDirPath, fs, oldDirs, path, readline, rl, root

### Community 17 - "Community 17"
Cohesion: 0.33
Nodes (5): uploadScan(), Scan, ScanPhoto, ScanStatus, UploadScanResponse

### Community 19 - "Community 19"
Cohesion: 0.29
Nodes (6): compilerOptions, paths, strict, extends, include, @/*

### Community 21 - "Community 21"
Cohesion: 0.53
Nodes (4): DocumentGroupContainers, Documents, Version, WorkspaceRootPath

### Community 24 - "Community 24"
Cohesion: 0.4
Nodes (4): editor.codeActionsOnSave, source.fixAll, source.organizeImports, source.sortMembers

### Community 37 - "Community 37"
Cohesion: 0.08
Nodes (24): 1. API, 2. Mobile, API Reference, Architecture, Auth — `/auth` (no token required), Children — `/children` (all require auth), code:block1 (MobroAI/), code:block2 (┌──────────────────────┐      HTTPS / JWT      ┌────────────) (+16 more)

### Community 38 - "Community 38"
Cohesion: 0.22
Nodes (8): code:block1 (apps/mobile/   React Native (Expo) — see apps/mobile/CLAUDE.), code:sh (# Mobile), graphify, Key decisions, Layout, MobroAI Monorepo — Claude Code Guide, Refer to app-level guides, Working in each app

### Community 39 - "Community 39"
Cohesion: 0.4
Nodes (4): DocumentGroupContainers, Documents, Version, WorkspaceRootPath

### Community 40 - "Community 40"
Cohesion: 0.4
Nodes (4): main, name, private, version

### Community 41 - "Community 41"
Cohesion: 0.4
Nodes (5): devDependencies, eslint, eslint-config-expo, @types/react, typescript

### Community 42 - "Community 42"
Cohesion: 0.5
Nodes (3): ExpandedNodes, PreviewInSolutionExplorer, SelectedNode

## Knowledge Gaps
- **209 isolated node(s):** `PreToolUse`, `allow`, `ExpandedNodes`, `SelectedNode`, `PreviewInSolutionExplorer` (+204 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Community 2` to `Community 40`?**
  _High betweenness centrality (0.009) - this node is a cross-community bridge._
- **Why does `useAuthStore` connect `Community 0` to `Community 5`?**
  _High betweenness centrality (0.005) - this node is a cross-community bridge._
- **Why does `Colors` connect `Community 0` to `Community 12`?**
  _High betweenness centrality (0.003) - this node is a cross-community bridge._
- **Are the 4 inferred relationships involving `MobroLens API Claude Guide` (e.g. with `API CI Workflow` and `MobroLens API AGENTS.md`) actually correct?**
  _`MobroLens API Claude Guide` has 4 INFERRED edges - model-reasoned connections that need verification._
- **Are the 3 inferred relationships involving `Mobile App Claude Guide` (e.g. with `Mobile CI Workflow` and `Mobile App AGENTS.md`) actually correct?**
  _`Mobile App Claude Guide` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `PreToolUse`, `allow`, `ExpandedNodes` to the rest of the system?**
  _209 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08 - nodes in this community are weakly interconnected._