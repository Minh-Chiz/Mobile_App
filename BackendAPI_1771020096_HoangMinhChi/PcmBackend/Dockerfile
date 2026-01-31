# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file csproj và restore các package
COPY ["PcmBackend.csproj", "./"]
RUN dotnet restore "PcmBackend.csproj"

# Copy toàn bộ code và build
COPY . .
RUN dotnet build "PcmBackend.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "PcmBackend.csproj" -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PcmBackend.dll"]