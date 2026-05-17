# Dev Environment Setup

## Prerequisites

| Tool | Version | Purpose |
|------|---------|---------|
| Node.js | 22+ | Mobile app |
| npm | 10+ | Mobile dependencies |
| .NET SDK | 10.0 | API |
| SQL Server Express | any | Database |
| Expo Go (phone) | latest | Running on device |

## First-time Setup

### 1. Clone the repo
```sh
git clone https://github.com/zanibul-shanto/MobroAI.git
cd MobroAI
```

### 2. API — local config

Create `apps/api/appsettings.Development.json` (gitignored):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER\\SQLEXPRESS;Database=MorboLens;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "MobroLens",
    "Audience": "MobroLensApp"
  }
}
```

Run migrations and start:
```sh
cd apps/api
dotnet ef database update
dotnet run
```

### 3. Mobile — local config

Create `apps/mobile/.env.local` (gitignored):
```
EXPO_PUBLIC_API_URL=http://localhost:5009
```

Install and start:
```sh
cd apps/mobile
npm install
npx expo start
```

Scan the QR code with Expo Go on your phone, or press `w` for web.
