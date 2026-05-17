# API Reference

Base URL: `http://localhost:5009` (dev) | configured via `EXPO_PUBLIC_API_URL`

All protected routes require `Authorization: Bearer <token>` header.

## Auth

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/auth/register` | No | Create account |
| POST | `/auth/login` | No | Get JWT token |
| POST | `/auth/forgot-password` | No | Request reset code |
| POST | `/auth/reset-password` | No | Reset with code |

## Users

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/users` | Yes | List all users (Admin) |
| GET | `/users/{id}` | Yes | Get user by ID |
| PUT | `/users/{id}` | Yes | Update user |
| DELETE | `/users/{id}` | Yes | Delete user |
| POST | `/users/{id}/change-password` | Yes | Change password |

## Children

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/children` | Yes | List children for current user |
| POST | `/children` | Yes | Create child record |
| PUT | `/children/{id}` | Yes | Update child |
| DELETE | `/children/{id}` | Yes | Delete child |

## Measles Scans

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/scans` | Yes | Create scan record |
| POST | `/scans/{id}/photos` | Yes | Upload photo |
| GET | `/scans/{id}` | Yes | Get scan + status |
| PUT | `/scans/{id}/status` | Yes | Update scan status |
| GET | `/scans/{id}/photos` | Yes | Get photos for scan |

## Location Logs

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/location-logs` | Yes | Log GPS position |
| GET | `/location-logs` | Yes | Get logs for current user |

## Roles

| Value | Name |
|-------|------|
| 0 | Admin |
| 1 | HealthCareOfficer |
| 2 | Parent |

## Scan Status

| Value | Name |
|-------|------|
| 0 | Pending |
| 1 | AI_Confirmed |
| 2 | Officer_Verified |
| 3 | Cleared |
