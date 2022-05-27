FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EasySynthesis.Console/EasySynthesis.Console.csproj", "EasySynthesis.Console/"]
RUN dotnet restore "EasySynthesis.Console/EasySynthesis.Console.csproj"
COPY . .
WORKDIR "/src/EasySynthesis.Console"
RUN dotnet build "EasySynthesis.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EasySynthesis.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasySynthesis.Console.dll"]
