FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY BKind.Web.sln ./
COPY BKind.Web/BKind.Web.csproj BKind.Web/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/BKind.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BKind.Web.dll"]
