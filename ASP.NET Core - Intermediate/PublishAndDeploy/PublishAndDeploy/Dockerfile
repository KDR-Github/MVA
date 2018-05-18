FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY PublishAndDeploy/PublishAndDeploy.csproj PublishAndDeploy/
RUN dotnet restore PublishAndDeploy/PublishAndDeploy.csproj
COPY . .
WORKDIR /src/PublishAndDeploy
RUN dotnet build PublishAndDeploy.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PublishAndDeploy.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PublishAndDeploy.dll"]
