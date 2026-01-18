# 1. Aşama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala
COPY . .

# Bağımlılıkları yükle ve projeyi derle
RUN dotnet restore "src/FlowEngine.API/FlowEngine.API.csproj"
RUN dotnet publish "src/FlowEngine.API/FlowEngine.API.csproj" -c Release -o /app/publish

# 2. Aşama: Çalıştırma (Runtime) - Daha küçük bir imaj kullanıyoruz
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "FlowEngine.API.dll"]