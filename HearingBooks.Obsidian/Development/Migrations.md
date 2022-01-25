# Migrations
Creating new migration:
```shell
dotnet ef migrations add <migration_name> -p HearingBooks.Persistance -s HearingBooks.Api
```

Updating database:
```shell
dotnet ef database update -p HearingBooks.Persistance -s HearingBooks.Api
```