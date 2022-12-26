#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SMS_Reminder/SMS_Reminder.csproj", "SMS_Reminder/"]
RUN dotnet restore "SMS_Reminder/SMS_Reminder.csproj"
COPY . .
WORKDIR "/src/SMS_Reminder"
RUN dotnet build "SMS_Reminder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMS_Reminder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMS_Reminder.dll"]