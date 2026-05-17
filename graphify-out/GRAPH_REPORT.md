# Graph Report - .  (2026-05-17)

## Corpus Check
- Corpus is ~31,805 words - fits in a single context window. You may not need a graph.

## Summary
- 410 nodes · 511 edges · 37 communities (21 shown, 16 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 26 edges (avg confidence: 0.9)
- Token cost: 24,566 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Auth & User Management|Auth & User Management]]
- [[_COMMUNITY_Measles Scan Pipeline|Measles Scan Pipeline]]
- [[_COMMUNITY_Child Records|Child Records]]
- [[_COMMUNITY_Location Tracking|Location Tracking]]
- [[_COMMUNITY_API Endpoints|API Endpoints]]
- [[_COMMUNITY_Mobile Navigation|Mobile Navigation]]
- [[_COMMUNITY_Database Schema|Database Schema]]
- [[_COMMUNITY_JWT & Token Service|JWT & Token Service]]
- [[_COMMUNITY_EF Core & AppDbContext|EF Core & AppDbContext]]
- [[_COMMUNITY_React Native UI Components|React Native UI Components]]
- [[_COMMUNITY_Zustand Auth Store|Zustand Auth Store]]
- [[_COMMUNITY_Background Location|Background Location]]
- [[_COMMUNITY_Translations & i18n|Translations & i18n]]
- [[_COMMUNITY_API Client (Axios)|API Client (Axios)]]
- [[_COMMUNITY_Expo Router Layout|Expo Router Layout]]
- [[_COMMUNITY_Type Definitions|Type Definitions]]
- [[_COMMUNITY_README Documentation|README Documentation]]
- [[_COMMUNITY_Config & Theme|Config & Theme]]
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

## God Nodes (most connected - your core abstractions)
1. `dependencies` - 32 edges
2. `expo` - 14 edges
3. `MobroLens API Claude Guide` - 14 edges
4. `useAuthStore` - 13 edges
5. `Mobile App Claude Guide` - 12 edges
6. `MeaslesScanEndpoints` - 11 edges
7. `Colors` - 11 edges
8. `API (ASP.NET Core 10)` - 11 edges
9. `ChildEndpoints` - 8 edges
10. `TodoEndpoints` - 8 edges

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

## Communities (37 total, 16 thin omitted)

### Community 0 - "Auth & User Management"
Cohesion: 0.07
Nodes (38): api, LocationData, LoginForm, loginSchema, LoginScreen(), styles, RegisterForm, registerSchema (+30 more)

### Community 1 - "Measles Scan Pipeline"
Cohesion: 0.09
Nodes (39): MobroLens API AGENTS.md, MobroLens API Claude Guide, MorboLens API README, MobroAI Monorepo Claude Guide, AI-Powered Measles Triage, Zustand Auth Store (expo-secure-store backed), Axios Request Interceptor (auto-JWT injection), Background Location Tracking Service (+31 more)

### Community 2 - "Child Records"
Cohesion: 0.06
Nodes (32): dependencies, axios, expo, expo-constants, expo-font, expo-haptics, expo-image-picker, expo-linking (+24 more)

### Community 3 - "Location Tracking"
Cohesion: 0.09
Nodes (30): API (ASP.NET Core 10), AppDbContext, Auth Endpoints (/auth), Axios, BCrypt, CHILD Entity, Children Endpoints (/children), EF Core (+22 more)

### Community 4 - "API Endpoints"
Cohesion: 0.07
Nodes (27): backgroundColor, backgroundImage, foregroundImage, monochromeImage, adaptiveIcon, edgeToEdgeEnabled, permissions, predictiveBackGestureEnabled (+19 more)

### Community 5 - "Mobile Navigation"
Cohesion: 0.13
Nodes (14): saveLocation(), InitialLayout(), unstable_settings, Config, Language, TranslationKeys, translations, LanguageContext (+6 more)

### Community 6 - "Database Schema"
Cohesion: 0.12
Nodes (16): devDependencies, eslint, eslint-config-expo, @types/react, typescript, main, name, private (+8 more)

### Community 7 - "JWT & Token Service"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 8 - "EF Core & AppDbContext"
Cohesion: 0.15
Nodes (12): AllowedHosts, ConnectionStrings, DefaultConnection, Jwt, AccessTokenExpirationMinutes, Audience, Issuer, Key (+4 more)

### Community 9 - "React Native UI Components"
Cohesion: 0.15
Nodes (7): BaseEntity, Child, LocationLog, MeaslesScan, ScanPhoto, Todo, User

### Community 10 - "Zustand Auth Store"
Cohesion: 0.23
Nodes (12): API App (ASP.NET Core 10), API CLAUDE.md Guide, Mobile App (React Native / Expo), Mobile CLAUDE.md Guide, appsettings.Development.json (gitignored), Path-Filtered CI Strategy, MobroAI Monorepo Claude Code Guide, Docs Directory (+4 more)

### Community 12 - "Translations & i18n"
Cohesion: 0.28
Nodes (5): HapticTab(), IconMapping, IconSymbol(), IconSymbolName, MAPPING

### Community 16 - "README Documentation"
Cohesion: 0.22
Nodes (7): exampleDirPath, fs, oldDirs, path, readline, rl, root

### Community 17 - "Config & Theme"
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

## Knowledge Gaps
- **165 isolated node(s):** `allow`, `Default`, `Microsoft.AspNetCore`, `AllowedHosts`, `DefaultConnection` (+160 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **16 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Child Records` to `Database Schema`?**
  _High betweenness centrality (0.012) - this node is a cross-community bridge._
- **Why does `useAuthStore` connect `Auth & User Management` to `Mobile Navigation`?**
  _High betweenness centrality (0.007) - this node is a cross-community bridge._
- **Why does `Colors` connect `Auth & User Management` to `Translations & i18n`?**
  _High betweenness centrality (0.004) - this node is a cross-community bridge._
- **Are the 4 inferred relationships involving `MobroLens API Claude Guide` (e.g. with `API CI Workflow` and `MobroLens API AGENTS.md`) actually correct?**
  _`MobroLens API Claude Guide` has 4 INFERRED edges - model-reasoned connections that need verification._
- **Are the 3 inferred relationships involving `Mobile App Claude Guide` (e.g. with `Mobile CI Workflow` and `Mobile App AGENTS.md`) actually correct?**
  _`Mobile App Claude Guide` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `allow`, `Default`, `Microsoft.AspNetCore` to the rest of the system?**
  _165 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Auth & User Management` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._