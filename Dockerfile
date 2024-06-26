﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SingularisTestTask/SingularisTestTask.csproj", "SingularisTestTask/"]
RUN dotnet restore "SingularisTestTask/SingularisTestTask.csproj"
COPY . .
WORKDIR "/src/SingularisTestTask"
RUN dotnet build "SingularisTestTask.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SingularisTestTask.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SingularisTestTask.dll"]
