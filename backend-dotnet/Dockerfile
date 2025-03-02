FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BackendAPI/*.csproj", "BackendAPI/"]
RUN dotnet restore "BackendAPI/BackendAPI.csproj"

# Copy the rest of the code
COPY . .

WORKDIR "/src/BackendAPI"
RUN dotnet build "BackendAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendAPI.dll"] 