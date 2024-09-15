FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestTask.WebApi.csproj", "src/TestTask.WebApi/"]
RUN dotnet restore "src/TestTask.WebApi/TestTask.WebApi.csproj"
WORKDIR "/src/TestTask.WebApi"
COPY . .
RUN dotnet build "TestTask.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTask.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTask.WebApi.dll"]