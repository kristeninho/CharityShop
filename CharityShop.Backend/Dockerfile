# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CharityShop.WebAPI/CharityShop.WebAPI.csproj", "CharityShop.WebAPI/"]
COPY ["CharityShop.Exceptions/CharityShop.Exceptions.csproj", "CharityShop.Exceptions/"]
COPY ["CharityShop.Contracts/CharityShop.Contracts.csproj", "CharityShop.Contracts/"]
COPY ["CharityShop.Common/CharityShop.Common.csproj", "CharityShop.Common/"]
COPY ["CharityShop.Data/CharityShop.Data.csproj", "CharityShop.Data/"]
COPY ["CharityShop.Services/CharityShop.Services.csproj", "CharityShop.Services/"]
RUN dotnet restore "CharityShop.WebAPI/CharityShop.WebAPI.csproj"
COPY . .
WORKDIR "/src/CharityShop.WebAPI"
RUN dotnet build "CharityShop.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CharityShop.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CharityShop.WebAPI.dll"]
