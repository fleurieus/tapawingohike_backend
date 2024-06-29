# Docker compose in this project
This starts the project on a different URL then localhost:
- Open [Tapawingo.localhost/api](Tapawingo.localhost/api) for the api
- Open [Tapawingo.localhost/admin](Tapawingo.localhost/admin) for the admin panel

## Development

First, run the development server:

```bash
# Up dev
docker compose -f docker-compose.dev.yml up
```

Open [Tapawingo.localhost/admin](Tapawingo.localhost/admin) with your browser to see the result.


## Production

Multistage builds are highly recommended in production. Combined with the Next [Output Standalone](https://nextjs.org/docs/advanced-features/output-file-tracing#automatically-copying-traced-files) feature, only `node_modules` files required for production are copied into the final Docker image.

First, run the production server (Final image approximately 110 MB).

```bash
# Build prod
docker compose -f docker-compose.prod.yml build

# Up prod in detached mode
docker compose -f docker-compose.prod.yml up -d
```

Open [Tapawingo.localhost/admin](Tapawingo.localhost/admin).

## DataBase
The development database is mounted and stored in the folder TapawingoDB. If you want a persistent database on the host machines file system on production copy the database part from docker-compose.dev.yml over to docker-compose.prod.yml