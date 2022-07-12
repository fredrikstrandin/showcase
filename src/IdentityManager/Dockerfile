#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IdentityManager/IdentityManager.csproj", "src/IdentityManager/"]
COPY ["src/CommonLib/CommonLib.csproj", "src/CommonLib/"]
COPY ["src/MessageLibrary/MessageLibrary.csproj", "src/MessageLibrary/"]
RUN dotnet restore "src/IdentityManager/IdentityManager.csproj"
COPY . .
WORKDIR "/src/src/IdentityManager"
RUN dotnet build "IdentityManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityManager.dll"]