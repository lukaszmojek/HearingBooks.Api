# Migrations
Creating new migration:
```shell
dotnet ef migrations add <migration_name> -p EasySynthesis.Persistance -s EasySynthesis.Api
```

Updating database:
```shell
dotnet ef database update -p EasySynthesis.Persistance -s EasySynthesis.Api
```