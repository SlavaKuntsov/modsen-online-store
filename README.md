# online-store

Modsen Online Store is a modular e-commerce backend built with **ASP.NET Core** and follows clean architecture principles. It exposes a RESTful API, uses EF Core for data access, Redis for background queues, and MinIO for object storage. The solution is container-friendly and comes with Docker Compose files for local development.

```sh
https://localhost:7000/scalar/
```

## Project structure

```text
.
├── docs                   # project documentation
├── mes                    # additional educational materials
└── src
    └── server
        ├── OnlineStore.API          # ASP.NET Core Web API entry point
        ├── OnlineStore.Application  # business logic, CQRS handlers
        ├── OnlineStore.Domain       # domain entities and value objects
        ├── OnlineStore.Persistance  # EF Core context and migrations
        ├── SharedKernel             # cross-cutting utilities (common, redis, minio, etc.)
        ├── Minios                   # MinIO helper library
        ├── OnlineStore.Tests        # automated tests
        └── docker-compose*.yml      # infrastructure services
```

## Architecture

The backend is organized into layers to keep concerns separated. External clients communicate with the API, which delegates work to the Application layer. Application logic coordinates Domain models and Persistence abstractions. SharedKernel provides integrations such as Redis and MinIO.

```text
Client -> API -> Application -> Domain
                     |
                     v
               Persistence -> Database
                     |
                     v
               SharedKernel (Redis/MinIO)
```

## Meow

```
⢠⠊⣉⠒⠤⢀⡀          ⡐⢁⠴⢜⢄
 ⡎⢸  ⠉⠐⠢⢌⠑⢄    ⡸  ⡆   ⠣⠱⡀
 ⡇⢸        ⣀⠗  ⠉⠉⠁  ⠙⠢⠤⡀⢃⢱
 ⡇⠘⣄⢀⠔⠉                    ⠈⠁⠘⡄
 ⢇    ⠁                          ⠘⡄
 ⢸            ⢀⣀⣀⡀        ⢀⣀⣀⡀  ⢣
 ⡸         ⢴⣾⡿⠿⠽⠇        ⠘⠛⠛⠛   ⢄
⠰⡁              ⢠⠒⠢⡀⠈⠒⠊      ⡠⢄ ⡘
 ⠱⣀          ⢀⠜    ⠇        ⢀⠔⠁ ⡏
     ⠑⠤⢄⣀⠔⠁     ⡜        ⠊⠁  ⢀⠜
```

## Features

- Asynchronous email sending via a Redis-backed queue

---

##### P.S

`.env` files in `./src/` directory
