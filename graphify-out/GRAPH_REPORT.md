# Graph Report - .  (2026-05-17)

## Corpus Check
- 84 files · ~36,635 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 374 nodes · 464 edges · 36 communities (20 shown, 16 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 18 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Mobile API Client Layer|Mobile API Client Layer]]
- [[_COMMUNITY_NPM Package Dependencies|NPM Package Dependencies]]
- [[_COMMUNITY_Project Docs & Architecture|Project Docs & Architecture]]
- [[_COMMUNITY_App Config & Assets|App Config & Assets]]
- [[_COMMUNITY_Mobile Routing & Layouts|Mobile Routing & Layouts]]
- [[_COMMUNITY_API Launch & Env Config|API Launch & Env Config]]
- [[_COMMUNITY_API JWT & DB Config|API JWT & DB Config]]
- [[_COMMUNITY_Domain Models|Domain Models]]
- [[_COMMUNITY_Measles Scan Endpoints|Measles Scan Endpoints]]
- [[_COMMUNITY_Child Endpoints|Child Endpoints]]
- [[_COMMUNITY_Tab Navigation UI|Tab Navigation UI]]
- [[_COMMUNITY_Project Reset Script|Project Reset Script]]
- [[_COMMUNITY_Todo Endpoints|Todo Endpoints]]
- [[_COMMUNITY_User Endpoints|User Endpoints]]
- [[_COMMUNITY_Auth Endpoints|Auth Endpoints]]
- [[_COMMUNITY_NPM Scripts|NPM Scripts]]
- [[_COMMUNITY_TypeScript Config|TypeScript Config]]
- [[_COMMUNITY_Location Log Endpoints|Location Log Endpoints]]
- [[_COMMUNITY_VS Code Settings|VS Code Settings]]
- [[_COMMUNITY_Claude Permissions Config|Claude Permissions Config]]
- [[_COMMUNITY_Brand & App Icons|Brand & App Icons]]
- [[_COMMUNITY_JWT Token Service|JWT Token Service]]
- [[_COMMUNITY_Database Context|Database Context]]
- [[_COMMUNITY_ESLint Config|ESLint Config]]
- [[_COMMUNITY_Auth Layout|Auth Layout]]
- [[_COMMUNITY_iOS Icon Component|iOS Icon Component]]
- [[_COMMUNITY_Shared Styles|Shared Styles]]
- [[_COMMUNITY_Base Entity|Base Entity]]
- [[_COMMUNITY_Auth Models|Auth Models]]
- [[_COMMUNITY_Mobile App README|Mobile App README]]
- [[_COMMUNITY_Mobile README|Mobile README]]
- [[_COMMUNITY_Splash Icon Asset|Splash Icon Asset]]

## God Nodes (most connected - your core abstractions)
1. `dependencies` - 32 edges
2. `expo` - 14 edges
3. `MobroLens API Claude Guide` - 14 edges
4. `useAuthStore` - 13 edges
5. `Mobile App Claude Guide` - 12 edges
6. `MeaslesScanEndpoints` - 11 edges
7. `Colors` - 11 edges
8. `ChildEndpoints` - 8 edges
9. `TodoEndpoints` - 8 edges
10. `UserEndpoints` - 8 edges

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
- **Measles AI Scan Pipeline** — concept_ai_measles_triage, concept_scan_lifecycle, entity_endpoints_dir, entity_appdbcontext [INFERRED 0.85]
- **Mobile Authentication Flow** — concept_auth_store, concept_jwt_auth, concept_axios_interceptor, entity_api_client [INFERRED 0.90]
- **Android Adaptive Icon Asset Set** — image_android_bg, image_android_fg, image_android_mono [EXTRACTED 1.00]

## Communities (36 total, 16 thin omitted)

### Community 0 - "Mobile API Client Layer"
Cohesion: 0.07
Nodes (38): api, LoginForm, loginSchema, LoginScreen(), styles, RegisterForm, registerSchema, ROLES (+30 more)

### Community 1 - "NPM Package Dependencies"
Cohesion: 0.11
Nodes (34): MobroLens API AGENTS.md, MobroLens API Claude Guide, MorboLens API README, MobroAI Monorepo Claude Guide, AI-Powered Measles Triage, Zustand Auth Store (expo-secure-store backed), Axios Request Interceptor (auto-JWT injection), Background Location Tracking Service (+26 more)

### Community 2 - "Project Docs & Architecture"
Cohesion: 0.06
Nodes (32): dependencies, axios, expo, expo-constants, expo-font, expo-haptics, expo-image-picker, expo-linking (+24 more)

### Community 3 - "App Config & Assets"
Cohesion: 0.07
Nodes (27): backgroundColor, backgroundImage, foregroundImage, monochromeImage, adaptiveIcon, edgeToEdgeEnabled, permissions, predictiveBackGestureEnabled (+19 more)

### Community 4 - "Mobile Routing & Layouts"
Cohesion: 0.12
Nodes (15): LocationData, saveLocation(), InitialLayout(), unstable_settings, Config, Language, TranslationKeys, translations (+7 more)

### Community 5 - "API Launch & Env Config"
Cohesion: 0.12
Nodes (16): devDependencies, eslint, eslint-config-expo, @types/react, typescript, main, name, private (+8 more)

### Community 6 - "API JWT & DB Config"
Cohesion: 0.13
Nodes (15): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+7 more)

### Community 7 - "Domain Models"
Cohesion: 0.15
Nodes (12): AllowedHosts, ConnectionStrings, DefaultConnection, Jwt, AccessTokenExpirationMinutes, Audience, Issuer, Key (+4 more)

### Community 8 - "Measles Scan Endpoints"
Cohesion: 0.15
Nodes (7): BaseEntity, Child, LocationLog, MeaslesScan, ScanPhoto, Todo, User

### Community 9 - "Child Endpoints"
Cohesion: 0.23
Nodes (12): API App (ASP.NET Core 10), API CLAUDE.md Guide, Mobile App (React Native / Expo), Mobile CLAUDE.md Guide, appsettings.Development.json (gitignored), Path-Filtered CI Strategy, MobroAI Monorepo Claude Code Guide, Docs Directory (+4 more)

### Community 12 - "Todo Endpoints"
Cohesion: 0.22
Nodes (7): exampleDirPath, fs, oldDirs, path, readline, rl, root

### Community 15 - "NPM Scripts"
Cohesion: 0.33
Nodes (5): uploadScan(), Scan, ScanPhoto, ScanStatus, UploadScanResponse

### Community 17 - "Location Log Endpoints"
Cohesion: 0.29
Nodes (6): compilerOptions, paths, strict, extends, include, @/*

### Community 19 - "Claude Permissions Config"
Cohesion: 0.4
Nodes (4): IconMapping, IconSymbol(), IconSymbolName, MAPPING

### Community 20 - "Brand & App Icons"
Cohesion: 0.4
Nodes (4): editor.codeActionsOnSave, source.fixAll, source.organizeImports, source.sortMembers

### Community 22 - "Database Context"
Cohesion: 0.4
Nodes (5): Android Icon Background (Light blue background with design circle/grid guides), Android Icon Foreground (Blue 3D chevron/arrow logo on white), Android Icon Monochrome (Grey chevron/arrow on white for adaptive icon), Favicon (Small blue chevron/arrow on rounded light blue square), App Icon (Blue chevron/arrow logo on light blue background with design grid)

## Knowledge Gaps
- **155 isolated node(s):** `allow`, `Default`, `Microsoft.AspNetCore`, `AllowedHosts`, `DefaultConnection` (+150 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **16 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Project Docs & Architecture` to `API Launch & Env Config`?**
  _High betweenness centrality (0.014) - this node is a cross-community bridge._
- **Why does `useAuthStore` connect `Mobile API Client Layer` to `Mobile Routing & Layouts`?**
  _High betweenness centrality (0.008) - this node is a cross-community bridge._
- **Are the 4 inferred relationships involving `MobroLens API Claude Guide` (e.g. with `API CI Workflow` and `MobroLens API AGENTS.md`) actually correct?**
  _`MobroLens API Claude Guide` has 4 INFERRED edges - model-reasoned connections that need verification._
- **Are the 3 inferred relationships involving `Mobile App Claude Guide` (e.g. with `Mobile CI Workflow` and `Mobile App AGENTS.md`) actually correct?**
  _`Mobile App Claude Guide` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `allow`, `Default`, `Microsoft.AspNetCore` to the rest of the system?**
  _155 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Mobile API Client Layer` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._
- **Should `NPM Package Dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.11 - nodes in this community are weakly interconnected._