#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["PlantillaMicroServicios/PlantillaMicroServicios.csproj", "PlantillaMicroServicios/"]
RUN dotnet restore "PlantillaMicroServicios/PlantillaMicroServicios.csproj"
COPY . .
WORKDIR "/src/PlantillaMicroServicios"
RUN dotnet build "PlantillaMicroServicios.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlantillaMicroServicios.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlantillaMicroServicios.dll"]