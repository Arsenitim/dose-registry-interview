# Architecture tour

## What this is

A small ASP.NET Core + PostgreSQL backend for a fictional "dose registry":
it tracks workers and radiation exposure records for them, and exposes a
report that aggregates exposure by year. All data is synthetic — there's
no real company, workers, or measurements behind any of this.

## Domain model

Two entities:

- **`Worker`** — `Id`, `FullName`, `PersonalNumber` (unique).
- **`DoseRecord`** — `Id`, `WorkerId` (FK), `PeriodStart`, `PeriodEnd`
  (the measurement period the record covers), `DoseValueMsv` (recorded
  dose, in millisieverts). A worker has many dose records.

That's the entire schema — see `db/init/001_schema_and_seed.sql` for the
exact DDL and seed rows.

## Project layout

```
src/DoseRegistry.Api/
├── Program.cs                      application startup/wiring
├── Configuration/base_config.json  connection + logging config
├── Controllers/                    HTTP endpoints
├── Models/                         EF Core entities (Worker, DoseRecord)
├── Dtos/                           wire-format request/response types
├── Data/
│   ├── DoseRegistryDbContext.cs
│   └── EntityTypeConfigurations/   Fluent API mapping, one file per entity
├── Services/                       business logic behind the report endpoint
└── Middleware/                     global exception handling

src/DoseRegistry.Api.Tests/         xUnit tests (in-memory DB)
db/init/001_schema_and_seed.sql     schema + seed data, loaded by Postgres on first boot
```

## Conventions worth knowing before you read the code

- **No EF Core migrations.** There's no `Migrations/` folder and the app
  never calls `Database.Migrate()`/`EnsureCreated()` against the real
  database. The schema is owned entirely by the SQL script in `db/init/`,
  and `DoseRegistryDbContext` just maps onto those pre-existing tables via
  Fluent API (`IEntityTypeConfiguration<T>`, one class per entity, applied
  via `ApplyConfigurationsFromAssembly`). If you need a schema change,
  you'd edit the SQL script directly.
- **Config, not `ConnectionStrings`.** The connection string isn't read
  from the usual `ConnectionStrings:Default` key — it's assembled in
  `Program.cs` from a `Database:Connection:{Host,Port,Database,Username,
  Password}` section in `Configuration/base_config.json`.
- **Plain constructor DI.** Controllers take their dependencies (a
  `DbContext`, a service interface) through the constructor — nothing
  fancier.
- **DTOs, not entities, on the wire.** Controllers always project EF
  entities to a `Dto` type before returning them; entities are never
  serialized directly.
- **Minimal error handling.** `Middleware/ExceptionHandlingMiddleware.cs`
  catches unhandled exceptions and returns a generic
  `{ "error": "..." }` body with a 500. Expected failure cases (not
  found, invalid input) are handled per-endpoint with normal ASP.NET Core
  results (`NotFound()`, `BadRequest()`, etc.) — there's no centralized
  error-code system.
- **Default JSON casing.** Standard ASP.NET Core camelCase, no custom
  serialization.

## Layer summary

`WorkersController` and `DoseRecordsController` are standard CRUD over the
two entities. `ReportsController` delegates to
`Services/AnnualSummaryReportService.cs`, which aggregates a worker's dose
records into a per-year total for the `/reports/annual-summary` endpoint.
