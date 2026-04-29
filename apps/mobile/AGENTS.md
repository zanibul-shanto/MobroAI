# AGENTS.md

## Stack
- Expo ~54.0.33 with expo-router ~6.0.23 (file-based routing)
- React Native 0.81.5, React 19.1.0, TypeScript ~5.9.2
- New Architecture enabled (`newArchEnabled: true`)
- React Compiler enabled (`experiments.reactCompiler: true`)

## Commands
- `npx expo start` — dev server (or `npm run android|ios|web`)
- `npm run lint` — runs `expo lint` (not plain eslint)
- No test framework configured; no typecheck script (relies on TS strict mode)

## Structure
- `app/` — routes (expo-router file-based routing); `_layout.tsx` is root layout
- `@/*` path alias maps to project root (set in tsconfig.json)
- `scripts/reset-project.js` — moves starter code to `app-example/`, resets `app/`

## Gotchas
- `main` entry point is `expo-router/entry`, not a local file
- Typed Routes enabled — types generated in `.expo/types/` (gitignored)
- `ios/` and `android/` are generated; do not manually edit (gitignored)
- `dist/`, `.expo/`, `expo-env.d.ts` are build artifacts (gitignored)
