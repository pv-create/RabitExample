#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NewsLetter.Api/NewsLetter.Api.csproj", "NewsLetter.Api/"]
COPY ["Core/NewsLetter.Core.csproj", "Core/"]
RUN dotnet restore "NewsLetter.Api/NewsLetter.Api.csproj"
COPY . .
WORKDIR "/src/NewsLetter.Api"
RUN dotnet build "NewsLetter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsLetter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsLetter.Api.dll"]