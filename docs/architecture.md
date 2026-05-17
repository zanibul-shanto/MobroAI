# Architecture

## System Overview

```
┌─────────────────────┐         HTTPS/JWT        ┌─────────────────────┐
│   Mobile (Expo)     │ ───────────────────────▶  │  API (.NET 10)      │
│   apps/mobile/      │                           │  apps/api/          │
│                     │                           │                     │
│  Expo Router        │                           │  Minimal API        │
│  Zustand (auth)     │                           │  EF Core + SQL Svr  │
│  expo-location      │                           │  JWT Bearer Auth    │
│  React Hook Form    │                           │  BCrypt passwords   │
└─────────────────────┘                           └─────────────────────┘
```

## Auth Flow

1. Mobile calls `POST /auth/login` → receives JWT (24h expiry)
2. JWT stored in `expo-secure-store` via Zustand `authStore`
3. Axios interceptor attaches `Authorization: Bearer <token>` on every request
4. API validates JWT via `Microsoft.AspNetCore.Authentication.JwtBearer`

## Domain Model

- **User** — Parent / HealthCareOfficer / Admin roles
- **Child** — belongs to a Parent user
- **MeaslesScan** — photo analysis result (Pending → AI_Confirmed → Officer_Verified → Cleared)
- **ScanPhoto** — binary image stored in SQL Server (VARBINARY MAX)
- **LocationLog** — GPS coordinates with timestamp, optionally linked to a child visit

## API Base URL

- Development: `http://localhost:5009` (set via `EXPO_PUBLIC_API_URL` in `apps/mobile/.env.local`)
- Production: set `EXPO_PUBLIC_API_URL` in your Expo build environment

## Ports

| Service | URL |
|---------|-----|
| API HTTP | http://localhost:5009 |
| API HTTPS | https://localhost:7206 |
