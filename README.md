# Dose Registry (interview exercise)

A small, self-contained ASP.NET Core + PostgreSQL backend, built as a
technical interview exercise. It's not a real product — a tiny "dose
registry" tracking fictional workers and fictional radiation exposure
records, plus a report endpoint that aggregates them by year.

## Quick start

```bash
docker compose up --build
```

No local .NET SDK required. Once it's up:

- API: `http://localhost:8090`
- Swagger UI: `http://localhost:8090/swagger`

## Docs

- [`docs/architecture.md`](docs/architecture.md) — project structure and conventions
- [`docs/api-usage.md`](docs/api-usage.md) — running it, full endpoint list, example calls
