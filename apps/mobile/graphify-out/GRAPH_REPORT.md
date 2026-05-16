# Graph Report - .  (2026-05-16)

## Corpus Check
- Corpus is ~27,326 words - fits in a single context window. You may not need a graph.

## Summary
- 220 nodes · 317 edges · 19 communities (14 shown, 5 thin omitted)
- Extraction: 97% EXTRACTED · 3% INFERRED · 0% AMBIGUOUS · INFERRED: 10 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_App Screens & API Layer|App Screens & API Layer]]
- [[_COMMUNITY_App Config & Adaptive Icons|App Config & Adaptive Icons]]
- [[_COMMUNITY_Runtime Dependencies|Runtime Dependencies]]
- [[_COMMUNITY_Dev Tooling & Types|Dev Tooling & Types]]
- [[_COMMUNITY_Location Services & Layout|Location Services & Layout]]
- [[_COMMUNITY_Auth State & Home Screen|Auth State & Home Screen]]
- [[_COMMUNITY_Expo Router & Auth Navigation|Expo Router & Auth Navigation]]
- [[_COMMUNITY_Tab UI Components|Tab UI Components]]
- [[_COMMUNITY_Internationalization (i18n)|Internationalization (i18n)]]
- [[_COMMUNITY_Project Reset Script|Project Reset Script]]
- [[_COMMUNITY_Brand Assets|Brand Assets]]
- [[_COMMUNITY_TypeScript Configuration|TypeScript Configuration]]
- [[_COMMUNITY_VS Code Settings|VS Code Settings]]
- [[_COMMUNITY_ESLint Configuration|ESLint Configuration]]
- [[_COMMUNITY_Expo Device Registry|Expo Device Registry]]
- [[_COMMUNITY_Expo Local Settings|Expo Local Settings]]
- [[_COMMUNITY_VS Code Extensions|VS Code Extensions]]
- [[_COMMUNITY_Expo Cache Info|Expo Cache Info]]

## God Nodes (most connected - your core abstractions)
1. `dependencies` - 31 edges
2. `expo` - 14 edges
3. `React Native 0.81.5` - 14 edges
4. `React 19.1.0` - 14 edges
5. `expo-router (~6.0.23) - File-Based Routing` - 12 edges
6. `useAuthStore` - 11 edges
7. `Colors` - 10 edges
8. `AGENTS.md - Project Stack & Structure Guide` - 10 edges
9. `scripts` - 7 edges
10. `api` - 6 edges

## Surprising Connections (you probably didn't know these)
- `InitialLayout()` --calls--> `useAuthStore`  [EXTRACTED]
  app/_layout.tsx → store/authStore.ts
- `LoginScreen()` --calls--> `useAuthStore`  [EXTRACTED]
  app/(auth)/login.tsx → store/authStore.ts
- `ChildrenScreen()` --calls--> `useAuthStore`  [EXTRACTED]
  app/(tabs)/children.tsx → store/authStore.ts
- `DashboardScreen()` --calls--> `useAuthStore`  [EXTRACTED]
  app/(tabs)/index.tsx → store/authStore.ts
- `ProfileScreen()` --calls--> `useAuthStore`  [EXTRACTED]
  app/(tabs)/profile.tsx → store/authStore.ts

## Hyperedges (group relationships)
- **Android Adaptive Icon System (Background + Foreground + Monochrome)** — img_android_bg, img_android_fg, img_android_mono [INFERRED 0.95]
- **Core Technology Stack (React Native + Expo + expo-router)** — expo_framework, expo_router, react_native, react, typescript [EXTRACTED 1.00]
- **Experimental Performance Features (New Architecture + React Compiler)** — new_architecture, react_compiler, react [EXTRACTED 0.95]

## Communities (19 total, 5 thin omitted)

### Community 0 - "App Screens & API Layer"
Cohesion: 0.08
Nodes (28): api, LoginForm, loginSchema, styles, RegisterForm, registerSchema, ROLES, styles (+20 more)

### Community 1 - "App Config & Adaptive Icons"
Cohesion: 0.07
Nodes (27): backgroundColor, backgroundImage, foregroundImage, monochromeImage, adaptiveIcon, edgeToEdgeEnabled, permissions, predictiveBackGestureEnabled (+19 more)

### Community 2 - "Runtime Dependencies"
Cohesion: 0.07
Nodes (28): dependencies, axios, expo, expo-constants, expo-font, expo-haptics, expo-linking, expo-location (+20 more)

### Community 3 - "Dev Tooling & Types"
Cohesion: 0.11
Nodes (17): devDependencies, eslint, eslint-config-expo, @types/react, typescript, main, name, private (+9 more)

### Community 4 - "Location Services & Layout"
Cohesion: 0.18
Nodes (9): LocationData, saveLocation(), InitialLayout(), unstable_settings, Config, useLocationTracking(), { locations }, now (+1 more)

### Community 5 - "Auth State & Home Screen"
Cohesion: 0.21
Nodes (9): LoginScreen(), saveAuthData(), useAuthStore, ChildrenScreen(), DashboardScreen(), styles, ProfileScreen(), AuthState (+1 more)

### Community 6 - "Expo Router & Auth Navigation"
Cohesion: 0.22
Nodes (10): AGENTS.md - Project Stack & Structure Guide, app/ Directory - File-Based Routes, Expo (~54.0.33), expo-router (~6.0.23) - File-Based Routing, expo-router, React Native New Architecture (newArchEnabled), React Compiler (experiments.reactCompiler), README.md - Expo App Getting Started Guide (+2 more)

### Community 7 - "Tab UI Components"
Cohesion: 0.24
Nodes (7): HapticTab(), react, React 19.1.0, IconMapping, IconSymbol(), IconSymbolName, MAPPING

### Community 8 - "Internationalization (i18n)"
Cohesion: 0.28
Nodes (6): Language, TranslationKeys, translations, LanguageContext, LanguageContextType, LanguageProvider()

### Community 9 - "Project Reset Script"
Cohesion: 0.22
Nodes (7): exampleDirPath, fs, oldDirs, path, readline, rl, root

### Community 10 - "Brand Assets"
Cohesion: 0.38
Nodes (7): Android Icon Background - Light Blue Grid & Circle Guidelines, Android Icon Foreground - Blue Gradient Chevron/Caret Logo, Android Icon Monochrome - Grey Chevron/Caret Logo, Favicon - Small Blue Rounded Square with Chevron Logo, App Icon - Blue Gradient Chevron on Light Blue Icon Grid, Splash Icon - Concentric Circles Placeholder/Guidelines, MobroAI Brand Identity - Blue Chevron Logo System

### Community 11 - "TypeScript Configuration"
Cohesion: 0.29
Nodes (6): compilerOptions, paths, strict, extends, include, @/*

### Community 12 - "VS Code Settings"
Cohesion: 0.4
Nodes (4): editor.codeActionsOnSave, source.fixAll, source.organizeImports, source.sortMembers

## Knowledge Gaps
- **117 isolated node(s):** `name`, `slug`, `version`, `orientation`, `icon` (+112 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **5 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `Runtime Dependencies` to `App Screens & API Layer`, `Dev Tooling & Types`, `Expo Router & Auth Navigation`, `Tab UI Components`?**
  _High betweenness centrality (0.218) - this node is a cross-community bridge._
- **Why does `React 19.1.0` connect `Tab UI Components` to `App Screens & API Layer`, `Location Services & Layout`, `Auth State & Home Screen`, `Expo Router & Auth Navigation`, `Internationalization (i18n)`?**
  _High betweenness centrality (0.130) - this node is a cross-community bridge._
- **Why does `React Native 0.81.5` connect `App Screens & API Layer` to `Location Services & Layout`, `Auth State & Home Screen`, `Expo Router & Auth Navigation`, `Tab UI Components`?**
  _High betweenness centrality (0.105) - this node is a cross-community bridge._
- **What connects `name`, `slug`, `version` to the rest of the system?**
  _117 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `App Screens & API Layer` be split into smaller, more focused modules?**
  _Cohesion score 0.08 - nodes in this community are weakly interconnected._
- **Should `App Config & Adaptive Icons` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._
- **Should `Runtime Dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.07 - nodes in this community are weakly interconnected._