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

## Develop and debug in VS Code

The development container provides the .NET 8 SDK, C# tooling, and PostgreSQL;
you do not need to install .NET or PostgreSQL on the host.

Prerequisites:

- Git and Visual Studio Code
- Docker Engine with Compose (Linux), or Docker Desktop (macOS/Windows)
- The VS Code **Dev Containers** extension

To start:

1. Open this repository in VS Code.
2. Run **Dev Containers: Reopen in Container** from the Command Palette.
3. Wait for PostgreSQL to become healthy and for the initial NuGet restore to
   finish.
4. Open **Run and Debug**, select **Dose Registry API**, and press **F5**.
5. Set a breakpoint and call an endpoint from
   `http://localhost:8090/swagger`.

Run all tests from **Terminal > Run Task > test**, or in the container
terminal:

```bash
dotnet test DoseRegistry.sln
```

The first start downloads the required container images and may take several
minutes. Later starts reuse them. If startup fails, confirm that
`docker version` reports both a client and server and that host ports `5432`
and `8090` are free.

## Docs

- [`docs/architecture.md`](docs/architecture.md) — project structure and conventions
- [`docs/api-usage.md`](docs/api-usage.md) — running it, full endpoint list, example calls
