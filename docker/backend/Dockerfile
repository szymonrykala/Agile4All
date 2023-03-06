FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/backend/AgileApp/AgileApp.csproj", "AgileApp/"]
RUN dotnet restore "AgileApp/AgileApp.csproj"
COPY backend/ .
WORKDIR "/src/AgileApp"
RUN dotnet build "AgileApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgileApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet AgileApp.dll --urls=http://+:8000