# Project Architecture Rules (Non-Obvious Only)

- **Flat Structure**: Currently follows a flat structure for simplicity.
- **Future Growth**: `Models/` and `Repository/` folders exist as placeholders for a future Repository pattern implementation.
- **Statelessness**: The API is designed to be stateless, though the In-Memory DB acts as a volatile state store.
