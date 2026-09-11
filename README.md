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

### Inspect the database in VS Code

The development container includes Microsoft's **PostgreSQL** VS Code
extension. Open its elephant icon in the Activity Bar, add a connection, and
use these values:

| Setting | Value |
|---|---|
| Host | `db` |
| Port | `5432` |
| Database | `dose_registry` |
| Username | `dose_registry` |
| Password | `dose_registry` |
| SSL | Disabled |

The PostgreSQL view can browse tables and other database objects, run queries,
and visualize the schema. The extension runs inside the development container;
no PostgreSQL client or database administration application needs to be
installed on the host.

From a host terminal, you can also open a database shell using the `psql`
client already included in the PostgreSQL container:

```bash
docker compose exec db psql -U dose_registry -d dose_registry
```

The first start downloads the required container images and may take several
minutes. Later starts reuse them. If startup fails, confirm that
`docker version` reports both a client and server and that host ports `5432`
and `8090` are free.

## Clean up the development environment

First stop the debugger and run **Dev Containers: Reopen Folder Locally** in
VS Code. Then, from a host terminal in this repository, remove the project
containers, network, and PostgreSQL data volume:

```bash
docker compose -f docker-compose.yml -f .devcontainer/docker-compose.devcontainer.yml down -v --remove-orphans
```

This removes the running environment and its database state, but leaves images
cached for a faster next start.

On Linux, VS Code may create a project-specific image whose name begins with
`vsc-dose-registry-interview-` and ends with `-uid:latest`. It adjusts the
container user's UID to match the host user. Find it with:

```bash
docker image ls --filter "reference=vsc-dose-registry-interview*-uid:latest"
```

If an image is listed and is not in use, remove it using the displayed image
ID, for example:

```bash
docker image rm 7bc1525779e6
```

The ID is only an example; use the value shown on the current machine. VS Code
will recreate this image if the project is opened in a Dev Container again.

To also remove the downloaded base images, run:

```bash
docker image rm mcr.microsoft.com/devcontainers/dotnet:8.0-bookworm postgres:16-alpine
```

If the regular packaged API workflow was also used, `docker image ls` may show
an image named `dose-registry-interview-api`. It can be removed with:

```bash
docker image rm dose-registry-interview-api
```

Docker may refuse to remove an image that another container or project still
uses; that is safe. Avoid `docker system prune -a` for this cleanup because it
can remove resources belonging to unrelated projects.

## Docs

- [`docs/architecture.md`](docs/architecture.md) — project structure and conventions
- [`docs/api-usage.md`](docs/api-usage.md) — running it, full endpoint list, example calls
