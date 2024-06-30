# Docker compose in this project
This starts the project on a different URL then localhost:
- Open [Tapawingo.localhost/api](Tapawingo.localhost/api) for the api
- Open [Tapawingo.localhost/admin](Tapawingo.localhost/admin) for the admin panel

## Development

First, run the development server with automatic refresh and updates:
- note: this runs the container detached, alternatively you can run the command with up instead of watch but it will not always refresh properly when ran with up.

```bash
# Up dev
docker compose -f docker-compose.dev.yml watch
```

Then open [Tapawingo.localhost/admin](Tapawingo.localhost/admin) with your browser after you have ran the admin panel docker compose to see the result, or go to [Tapawingo.localhost/api/swagger](Tapawingo.localhost/api/swagger) to test the api through swagger.


## Production

First, run the production server.

```bash
# Build prod
docker compose -f docker-compose.prod.yml build

# Up prod in detached mode
docker compose -f docker-compose.prod.yml up -d
```

Then docker compose up the desired admin panel (development or production) and open [Tapawingo.localhost/admin](Tapawingo.localhost/admin).

## DataBase
The development database can be mounted and stored in the folder TapawingoDB. If you want a persistent database on the host machines file system or on production copy the database volumes from docker-compose.dev.yml over to docker-compose.prod.yml.