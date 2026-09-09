# Running it and calling the API

## Run

```bash
docker compose up --build
```

Starts PostgreSQL (schema + seed data loaded automatically on first boot
from `db/init/001_schema_and_seed.sql`) and the API.

- API: `http://localhost:8090`
- Swagger UI: `http://localhost:8090/swagger`

Reset to the original seed data:

```bash
docker compose down -v
docker compose up --build
```

Run tests without installing the .NET SDK locally:

```bash
docker compose run --rm sdk test DoseRegistry.Api.Tests/DoseRegistry.Api.Tests.csproj
```

## Endpoints

| Method | Route | Notes |
|---|---|---|
| GET | `/health` | Liveness check |
| GET | `/workers` | List all workers |
| GET | `/workers/{id}` | Get one worker |
| POST | `/workers` | Create a worker |
| GET | `/doserecords?workerId=` | List dose records, optionally filtered by worker |
| POST | `/doserecords` | Create a dose record |
| GET | `/reports/annual-summary?year=YYYY` | Per-worker total recorded dose for a calendar year |

## A few example calls

```bash
curl -s http://localhost:8090/health
# {"status":"ok"}

curl -s http://localhost:8090/workers
# [{"id":1,"fullName":"Anna Novak","personalNumber":"W-1001"}, ...]

curl -s http://localhost:8090/workers/1
# {"id":1,"fullName":"Anna Novak","personalNumber":"W-1001"}

curl -s -X POST http://localhost:8090/workers \
  -H "Content-Type: application/json" \
  -d '{"fullName":"Test Worker","personalNumber":"W-9001"}'
# 201, returns the created worker

curl -s "http://localhost:8090/doserecords?workerId=1"
# [{"id":1,"workerId":1,"periodStart":"2026-02-01","periodEnd":"2026-02-28","doseValueMsv":4.0}, ...]

curl -s -X POST http://localhost:8090/doserecords \
  -H "Content-Type: application/json" \
  -d '{"workerId":1,"periodStart":"2026-05-01","periodEnd":"2026-05-31","doseValueMsv":1.25}'
# 201, returns the created record
```

Swagger UI at `/swagger` covers all of the above interactively, including
`/reports/annual-summary`.
