# HearingBooks

[Trello board of the project](https://trello.com/b/mLnc89Du/hearingbooks)

---

## Requirements for running this (awesowe) project locally:

- .NET 6.0.x SDK
- IDE, I can recommend latest version of Rider (since Visual Studio is shitty) for Backend
- [NodeJS installed](https://nodejs.org/en/)
- [VS Code for Frontend](https://code.visualstudio.com/)
- [Docker installed](https://www.docker.com/get-started)
- [EF Core tools installed](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- [Azure Data Studio installed](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)
- Some computer more powerful than a potato (or not, surprise me)
- And some operating system on top of it I guess

---

## Setting up environment:

Go to the `docker` folder and execute the command `docker-compose up`

---

## Migrations

### NOTE:

To performs steps below, you need to be in the `HearingBooks.Api` folder

### Adding migrations

`dotnet ef migrations add {migration_name} --startup-project HearingBooks.Api --project HearingBooks.Persistance`

### Updating the database

`dotnet ef database update --project HearingBooksApi`
