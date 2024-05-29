# Tapawingo Backend

## Setup
First Make sure u already have a database in posgres, and add the authorization to the connectionstring: 
```json
"DefaultConnection": "server=localhost;database={database name};user={user from mysql};password={password for user}"
```

## Database tables
Then run command in the Package Manager Console (You can find it by using the search in the toolbar):
Command can differ between usage of IDE.

Visual Studio Code:
```bash
dotnet ef database update
```
Visual Studio 2022:
```bash
Update-Database
```

## Start backend: 
Visual Studio Code:
```bash
dotnet run
```

Visual Studio 2022:
You can use the play button in the toolbar, it needs to say (https).

## Packages
To add a NuGet package in Visual Studio Code:
```bash
dotnet add package {packageName}
```
For Visual Studio 2022(You can also use the NuGet Package Manager):
```bash
Install-Package {packageName}
```

## Migrations
### show all migrations: 
Visual Studio Code:
```bash
dotnet ef migrations list
```
Visual Studio 2022:
```bash
Get-Migrations
```

### Create new migration
Visual Studio Code:
```bash
dotnet ef migrations add {name of migration}
```
Visual Studio 2022:
```bash
Add-Migration {name of migration}
```
After command, dont forget to add new tables to the contextFile.

### Remove a made migration before setting it to the database
Visual Studio Code:
```bash
dotnet ef migrations remove
```
Visual Studio 2022:
```bash
Remove-Migration
```

### Revert to a specific migration
Visual Studio Code:
```bash
dotnet ef database update {name of migration where you want to go}
```
Visual Studio 2022:
```bash
Update-Database {name of migration where you want to go}
```
When executing this command, you will need to remove the migration that you want to go to.
To add change to dataset: 1. first revert 2. remove 3. add 4. run

## Steps to create a entire entity
1. Create Record in Dtos folder with the attributes of that entity

## Project structure
1. Models: Contains the entities that are used in the database.
2. Data: Contains the Database context file.
3. Dtos: Contains the Data Transfer Objects that are used to send data to the client.
4. Controllers: Contains the controllers that are used to handle the requests.
5. Services: Contains the services that are used to handle the business logic.
6. Repositories: Contains the repositories that are used to handle the database logic.

## API Endpoints
To see all the endpoints that are available in the API, you can go to the swagger page of the API(ApiUrl/swagger/index.html).
