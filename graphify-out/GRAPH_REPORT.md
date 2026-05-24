# Graph Report - .  (2026-05-24)

## Corpus Check
- 84 files · ~35,675 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 500 nodes · 600 edges · 47 communities (27 shown, 20 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 28 edges (avg confidence: 0.9)
- Token cost: 22,047 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Mobile App Core|Mobile App Core]]
- [[_COMMUNITY_API Architecture Docs|API Architecture Docs]]
- [[_COMMUNITY_Project Documentation|Project Documentation]]
- [[_COMMUNITY_Mobile Package Dependencies|Mobile Package Dependencies]]
- [[_COMMUNITY_README References|README References]]
- [[_COMMUNITY_Mobile App Config|Mobile App Config]]
- [[_COMMUNITY_MobroAI README|MobroAI README]]
- [[_COMMUNITY_Location & Layout|Location & Layout]]
- [[_COMMUNITY_Mobile Dev Dependencies|Mobile Dev Dependencies]]
- [[_COMMUNITY_API Launch Settings|API Launch Settings]]
- [[_COMMUNITY_API App Settings|API App Settings]]
- [[_COMMUNITY_API Domain Models|API Domain Models]]
- [[_COMMUNITY_Measles Scan Endpoints|Measles Scan Endpoints]]
- [[_COMMUNITY_Child Endpoints|Child Endpoints]]
- [[_COMMUNITY_Todo Endpoints|Todo Endpoints]]
- [[_COMMUNITY_User Endpoints|User Endpoints]]
- [[_COMMUNITY_CLAUDE.md Config|CLAUDE.md Config]]
- [[_COMMUNITY_Mobile Reset Scripts|Mobile Reset Scripts]]
- [[_COMMUNITY_Scan Types & API|Scan Types & API]]
- [[_COMMUNITY_Auth Endpoints|Auth Endpoints]]
- [[_COMMUNITY_Auth Endpoints Flow|Auth Endpoints Flow]]
- [[_COMMUNITY_Location Endpoints|Location Endpoints]]
- [[_COMMUNITY_Vaccine Endpoints|Vaccine Endpoints]]
- [[_COMMUNITY_CI Workflows|CI Workflows]]
- [[_COMMUNITY_Token Service|Token Service]]
- [[_COMMUNITY_Mobile Constants|Mobile Constants]]
- [[_COMMUNITY_Mobile Hooks|Mobile Hooks]]
- [[_COMMUNITY_Mobile Store|Mobile Store]]
- [[_COMMUNITY_Mobile Context|Mobile Context]]
- [[_COMMUNITY_Validation Schemas|Validation Schemas]]
- [[_COMMUNITY_Mobile UI Components|Mobile UI Components]]
- [[_COMMUNITY_ONNX Model Pipeline|ONNX Model Pipeline]]
- [[_COMMUNITY_EF Migrations|EF Migrations]]
- [[_COMMUNITY_VS Settings|VS Settings]]
- [[_COMMUNITY_Expo Config|Expo Config]]
- [[_COMMUNITY_Mobile App Images|Mobile App Images]]
- [[_COMMUNITY_Mobile Types|Mobile Types]]
- [[_COMMUNITY_Mobile Services|Mobile Services]]
- [[_COMMUNITY_Expo Types|Expo Types]]
- [[_COMMUNITY_Claude Settings|Claude Settings]]
- [[_COMMUNITY_API Solution Config|API Solution Config]]

## God Nodes (most connected - your core abstractions)
1. `dependencies` - 33 edges
2. `expo` - 14 edges
3. `MobroLens API Claude Guide` - 14 edges
4. `useAuthStore` - 13 edges
5. `Mobile App Claude Guide` - 12 edges
6. `API App (ASP.NET Core 10)` - 12 edges
7. `MeaslesScanEndpoints` - 11 edges
8. `Colors` - 11 edges
9. `Mobile App (Expo/React Native)` - 11 edges
10. `API (ASP.NET Core 10)` - 11 edges

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
- **End-to-End Request Path** — apps_mobile, jwt_auth, apps_api, morbolens_db [EXTRACTED 1.00]
- **Measles Scan Processing Pipeline** — apps_mobile, apps_api, scanphoto_imagedata, ml_model, scan_status_workflow [EXTRACTED 1.00]
- **ONNX Model Conversion Pipeline** — model_h5, convert_to_onnx_py, monkeynet_lite_onnx [EXTRACTED 1.00]

## Communities (47 total, 20 thin omitted)

### Community 0 - "Mobile App Core"
Cohesion: 0.06
Nodes (42): api, LoginForm, loginSchema, LoginScreen(), styles, RegisterForm, registerSchema, ROLES (+34 more)

### Community 1 - "API Architecture Docs"
Cohesion: 0.07
Nodes (38): api/api.ts (Mobile API Client), Architecture, Auth, code:bash (dotnet run          # HTTP on port 5009), Coding Conventions, Commands, Gotchas, API App (ASP.NET Core 10) (+30 more)

### Community 2 - "Project Documentation"
Cohesion: 0.09
Nodes (39): MobroLens API AGENTS.md, MobroLens API Claude Guide, MorboLens API README, MobroAI Monorepo Claude Guide, AI-Powered Measles Triage, Zustand Auth Store (expo-secure-store backed), Axios Request Interceptor (auto-JWT injection), Background Location Tracking Service (+31 more)

### Community 3 - "Mobile Package Dependencies"
Cohesion: 0.06
Nodes (33): dependencies, axios, expo, expo-constants, expo-font, expo-haptics, expo-image-picker, expo-linking (+25 more)

### Community 4 - "README References"
Cohesion: 0.09
Nodes (30): API (ASP.NET Core 10), AppDbContext, Auth Endpoints (/auth), Axios, BCrypt, CHILD Entity, Children Endpoints (/children), EF Core (+22 more)

### Community 5 - "Mobile App Config"
Cohesion: 0.07
Nodes (27): backgroundColor, backgroundImage, foregroundImage, monochromeImage, adaptiveIcon, edgeToEdgeEnabled, permissions, predictiveBackGestureEnabled (+19 more)

### Community 6 - "MobroAI README"
Cohesion: 0.08
Nodes (24): 1. API, 2. Mobile, API Reference, Architecture, Auth — `/auth` (no token required), Children — `/children` (all require auth), code:block1 (MobroAI/), code:block2 (┌──────────────────────┐      HTTPS / JWT      ┌────────────) (+16 more)

### Community 7 - "Location & Layout"
Cohesion: 0.12
Nodes (15): LocationData, saveLocation(), InitialLayout(), unstable_settings, Config, Language, TranslationKeys, translations (+7 more)

### Community 8 - "Mobile Dev Dependencies"
Cohesion: 0.12
Nodes (16): devDependencies, eslint, eslint-config-expo, @types/react, typescript, main, name, private (+8 more)

### Community 9 - "API Launch Settings"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 10 - "API App Settings"
Cohesion: 0.15
Nodes (12): AllowedHosts, ConnectionStrings, DefaultConnection, Jwt, AccessTokenExpirationMinutes, Audience, Issuer, Key (+4 more)

### Community 11 - "API Domain Models"
Cohesion: 0.15
Nodes (7): BaseEntity, Child, LocationLog, MeaslesScan, ScanPhoto, Todo, User

### Community 16 - "CLAUDE.md Config"
Cohesion: 0.22
Nodes (8): code:block1 (apps/mobile/   React Native (Expo) — see apps/mobile/CLAUDE.), code:sh (# Mobile), graphify, Key decisions, Layout, MobroAI Monorepo — Claude Code Guide, Refer to app-level guides, Working in each app

### Community 17 - "Mobile Reset Scripts"
Cohesion: 0.22
Nodes (7): exampleDirPath, fs, oldDirs, path, readline, rl, root

### Community 18 - "Scan Types & API"
Cohesion: 0.33
Nodes (5): uploadScan(), Scan, ScanPhoto, ScanStatus, UploadScanResponse

### Community 20 - "Auth Endpoints Flow"
Cohesion: 0.29
Nodes (7): API Client, Architecture, Auth, Background Location, Internationalization, Path Alias, Routing

### Community 21 - "Location Endpoints"
Cohesion: 0.29
Nodes (6): compilerOptions, paths, strict, extends, include, @/*

### Community 23 - "CI Workflows"
Cohesion: 0.53
Nodes (4): DocumentGroupContainers, Documents, Version, WorkspaceRootPath

### Community 25 - "Mobile Constants"
Cohesion: 0.4
Nodes (4): DocumentGroupContainers, Documents, Version, WorkspaceRootPath

### Community 27 - "Mobile Store"
Cohesion: 0.4
Nodes (4): editor.codeActionsOnSave, source.fixAll, source.organizeImports, source.sortMembers

### Community 30 - "Mobile UI Components"
Cohesion: 0.5
Nodes (3): ExpandedNodes, PreviewInSolutionExplorer, SelectedNode

## Knowledge Gaps
- **217 isolated node(s):** `PreToolUse`, `allow`, `ExpandedNodes`, `SelectedNode`, `PreviewInSolutionExplorer` (+212 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **20 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Mobile Package Dependencies` to `Mobile Dev Dependencies`?**
  _High betweenness centrality (0.008) - this node is a cross-community bridge._
- **Why does `useAuthStore` connect `Mobile App Core` to `Location & Layout`?**
  _High betweenness centrality (0.004) - this node is a cross-community bridge._
- **Are the 4 inferred relationships involving `MobroLens API Claude Guide` (e.g. with `API CI Workflow` and `MobroLens API AGENTS.md`) actually correct?**
  _`MobroLens API Claude Guide` has 4 INFERRED edges - model-reasoned connections that need verification._
- **Are the 3 inferred relationships involving `Mobile App Claude Guide` (e.g. with `Mobile CI Workflow` and `Mobile App AGENTS.md`) actually correct?**
  _`Mobile App Claude Guide` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `PreToolUse`, `allow`, `ExpandedNodes` to the rest of the system?**
  _217 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Mobile App Core` be split into smaller, more focused modules?**
  _Cohesion score 0.06 - nodes in this community are weakly interconnected._
- **Should `API Architecture Docs` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._