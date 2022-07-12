#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/NorthStarGraphQL/NorthStarGraphQL.csproj", "src/NorthStarGraphQL/"]
COPY ["src/MessageLibrary/MessageLibrary.csproj", "src/MessageLibrary/"]
RUN dotnet restore "src/NorthStarGraphQL/NorthStarGraphQL.csproj"
COPY . .
WORKDIR "/src/src/NorthStarGraphQL"
RUN dotnet build "NorthStarGraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NorthStarGraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NorthStarGraphQL.dll"]