# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
npx expo start          # dev server (scan QR with Expo Go or open in emulator)
npm run android         # launch on Android emulator
npm run ios             # launch on iOS simulator
npm run web             # launch in browser
npm run lint            # runs expo lint (not plain eslint — use this, not npx eslint)
```

No test framework or typecheck script is configured. TypeScript strict mode is the safety net.

## Architecture

### Routing
expo-router with file-based routing. `app/_layout.tsx` is the root; it contains `InitialLayout`, which handles all auth-based redirects before any screen renders. Two route groups:
- `app/(auth)/` — login, register, forgot-password (no auth required)
- `app/(tabs)/` — dashboard (index), children, profile (auth required)

The redirect logic lives entirely in `InitialLayout` via a `useEffect` that watches `isAuthenticated` and `segments`. No route-level guards exist — all protection flows through this single component.

### Auth
`store/authStore.ts` — Zustand store backed by `expo-secure-store`. Holds `user`, `token`, `isAuthenticated`, `isLoading`. Call `initialize()` once on mount (done in `InitialLayout`). Use `saveAuthData(token, user)` after login to persist credentials, then call `setUser` and `setToken` to update the store. Every tab screen reads from this store — changes to its shape break the entire app.

### API Client
`api/api.ts` — axios instance with base URL pointing to a dev tunnel. A request interceptor reads `auth_token` from SecureStore and injects `Authorization: Bearer <token>` automatically. The `X-Tunnel-Skip-AntiPhishing-Page` header is required for the dev tunnel and should stay in production until the backend has a stable URL.

### Background Location
`services/locationService.ts` — registers a `TaskManager` background task (`LOCATION_TRACKING_TASK`) that must be defined before the app mounts. It is side-effect imported in `app/_layout.tsx` (`import '@/services/locationService'`) to guarantee registration. The task rate-limits POSTs to the API using `Config.LOCATION_UPDATE_INTERVAL_MS`, persisting the last update timestamp in SecureStore so the interval survives app restarts.

### Internationalization
`context/LanguageContext.tsx` wraps the app at the root. Use `useLanguage()` to get `t(key)` for translations. Translation keys and strings live in `constants/translations.ts`.

### Path Alias
`@/*` maps to the project root (configured in `tsconfig.json`). Always use this alias for imports rather than relative paths.

## Key Constraints

- `ios/` and `android/` are gitignored and auto-generated — never manually edit.
- New Architecture (`newArchEnabled: true`) and React Compiler (`experiments.reactCompiler: true`) are both enabled in `app.json`. Avoid patterns that are known to break with these (e.g., mutating state directly).
- The `API_BASE_URL` in `api/api.ts` is a dev tunnel URL — it will need replacing for production or when the tunnel restarts.
