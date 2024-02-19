#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackendCRUD.Api/BackendCRUD.Api.csproj", "BackendCRUD.Api/"]
COPY ["BackendCRUD.Application/BackendCRUD.Application.csproj", "BackendCRUD.Application/"]
COPY ["BackendCRUD.Common/BackendCRUD.Common.csproj", "BackendCRUD.Common/"]
COPY ["BackendCRUD.Infrasctructure/BackendCRUD.Infrasctructure.csproj", "BackendCRUD.DataAccess/"]
RUN dotnet restore "BackendCRUD.Api/BackendCRUD.Api.csproj"
COPY . .
WORKDIR "/src/BackendCRUD.Api"
RUN dotnet build "BackendCRUD.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendCRUD.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendCRUD.Api.dll"]