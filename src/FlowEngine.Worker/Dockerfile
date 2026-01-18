# 1. Aşama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

# Worker projesini derle
RUN dotnet restore "src/FlowEngine.Worker/FlowEngine.Worker.csproj"
RUN dotnet publish "src/FlowEngine.Worker/FlowEngine.Worker.csproj" -c Release -o /app/publish

# 2. Aşama: Çalıştırma (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FlowEngine.Worker.dll"]