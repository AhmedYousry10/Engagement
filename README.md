# Engagement Site

A full-stack engagement party website: a public site (hero, live countdown,
event details, photo gallery, a WhatsApp guest-message form, admin-editable
site colors, and scroll/hover animations throughout) and an admin dashboard
for editing all of it.

- `backend/` — ASP.NET Core Web API (.NET 10) in a Clean Architecture layout
- `frontend/` — Angular (standalone components), Angular CLI project

## Prerequisites

- .NET SDK 10
- Node.js 20+ and npm
- A SQL Server instance (LocalDB, SQL Express, a container, or a full server)
- The `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

## Backend architecture

`backend/EngagementApi.slnx` ties together four projects under `backend/src/`:

- **`EngagementApi.Domain`** — entities only (`SiteContent`, `DetailCard`,
  `Photo`, `AdminUser`). No dependencies on anything else.
- **`EngagementApi.Application`** — the use cases: DTOs, service interfaces
  and their implementations (`SiteContentService`, `DetailsService`,
  `PhotosService`, `AuthService`), and the abstractions they depend on
  (`IAppDbContext`, `ITokenService`, `IPasswordHasherService`,
  `IPhotoStorageService`). Depends only on Domain.
- **`EngagementApi.Infrastructure`** — the technology-specific implementations
  of those abstractions: the EF Core `AppDbContext` and entity
  configurations, migrations, the JWT `TokenService`, the Identity-based
  `PasswordHasherService`, and the filesystem-based `FilePhotoStorageService`.
  Depends on Application.
- **`EngagementApi.Api`** — controllers and the composition root
  (`Program.cs`, `appsettings*.json`, `wwwroot`). Depends on Application
  (for the interfaces controllers call) and Infrastructure (to wire up DI).

Controllers only ever talk to `Application` interfaces — they don't know
`Infrastructure` exists. This means the persistence, auth, and file-storage
implementations can be swapped without touching a controller.

## Backend setup

1. Set your connection string in
   `backend/src/EngagementApi.Api/appsettings.Development.json`
   (`ConnectionStrings:DefaultConnection`). A LocalDB example is included by
   default — change it to point at whatever SQL Server instance you're using.
2. Apply the migration to create the database:

   ```bash
   cd backend
   dotnet ef database update --project src/EngagementApi.Infrastructure --startup-project src/EngagementApi.Api
   ```

3. Run the API:

   ```bash
   dotnet run --project src/EngagementApi.Api
   ```

   By default this serves on `https://localhost:7193` (see
   `src/EngagementApi.Api/Properties/launchSettings.json`). On first run it
   seeds:
   - A default `SiteContent` row, including default colors (edit all of it
     from the dashboard once you log in).
   - One admin user, from `SeedAdmin:Username` / `SeedAdmin:Password` in
     `appsettings.json` (defaults to `admin` / `ChangeMe123!` — **change this**
     before any real use, either via `PUT /api/auth/change-password` after
     logging in, or by editing the seed values before the first run).

   Uploaded photos are written to `backend/src/EngagementApi.Api/wwwroot/uploads`
   and served as static files at `/uploads/<file>`.

   Also set a real value for `Jwt:Key` (at least 32 characters) outside of
   local development — the checked-in dev key is for local use only.

### Adding a new migration later

```bash
cd backend
dotnet ef migrations add <Name> --project src/EngagementApi.Infrastructure --startup-project src/EngagementApi.Api -o Persistence/Migrations
```

## Frontend setup

```bash
cd frontend
npm install
npm start
```

Serves on `http://localhost:4200` by default. The API base URL is
configured in `frontend/src/environments/environment.development.ts`
(`apiUrl`, defaults to `https://localhost:7193/api`) — update it if you
changed the backend's port.

### Site colors & animations

- The dashboard's **Settings** tab has four color pickers (primary,
  secondary, background, text) with a live preview. They're stored on
  `SiteContent` (`colorPrimary`/`colorSecondary`/`colorBackground`/`colorText`)
  and applied on the public site as CSS custom properties
  (`--rose-deep`, `--sage`, `--ivory`, `--ink`) scoped to that page, so the
  admin dashboard's own styling is unaffected.
- The public site uses `provideRouter(routes, withViewTransitions())` for
  cross-fade route transitions, a scroll-reveal directive
  (`core/directives/reveal-on-scroll.directive.ts`, applied via `appReveal`)
  for staggered entrance animations on cards/gallery items, and hover
  transitions on buttons, cards, and gallery images. All of it respects
  `prefers-reduced-motion`.

## Running both together

Start the backend (`dotnet run --project src/EngagementApi.Api` in
`backend/`) and the frontend (`npm start` in `frontend/`) in two terminals.
The Angular dev server proxies nothing by default — it calls the API's
absolute URL directly, so both need to be running for the public site and
dashboard to load data.

### CORS

The API only accepts cross-origin requests from the origins listed under
`Cors:AllowedOrigins` in `appsettings.json` (defaults to
`http://localhost:4200`). If you serve the Angular app from a different
origin (a different port, a custom host, etc.), add it to that list.

## Production builds

- Backend: `dotnet publish -c Release src/EngagementApi.Api`
- Frontend: `npm run build` (outputs to `frontend/dist/frontend`); update
  `src/environments/environment.ts` (the production environment) with the
  real API URL before building.

In production, serve the Angular build as static files from whatever you
like (the API itself, a CDN, nginx, etc.) and point `apiUrl` at wherever the
API is actually reachable. Set `Cors:AllowedOrigins` on the API to match.
