FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj first for layer-cached restore
COPY WeekMap.csproj ./
RUN dotnet restore WeekMap.csproj

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
RUN groupadd --gid 1001 appgroup \
    && useradd --uid 1001 --gid 1001 --no-create-home --shell /bin/false appuser
WORKDIR /app
COPY --from=build --chown=appuser:appgroup /app/publish .
USER appuser
ENTRYPOINT ["dotnet", "WeekMap.dll"]
