FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7141

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY WeekMap.sln ./
COPY WeekMap.BackEnd.ASP.NET/ ./WeekMap.BackEnd.ASP.NET/
COPY WeekMap.BackEndTests.C#XUNIT/ ./WeekMap.BackEndTests.C#XUNIT/
COPY WeekMap.FrontEnd.REACT/ ./WeekMap.FrontEnd.REACT/
COPY WeekMap.Tests/ ./WeekMap.Tests/
COPY WeekMap.TestingScripts/ ./WeekMap.TestingScripts/

RUN dotnet restore

RUN dotnet test WeekMap.BackEndTests.C#XUNIT/XUnitTests.csproj --no-build --logger:trx

WORKDIR /src/WeekMap.BackEnd.ASP.NET
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WeekMap.BackEnd.ASP.NET.dll"]