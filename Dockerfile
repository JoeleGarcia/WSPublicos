# ----------------------------------------------------
# STAGE 1: Build 
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copia el archivo de la solución y todo el código fuente
COPY ["APEC.WSPublicos.sln", "."]
COPY . .

# 2. Restaura las dependencias
RUN dotnet restore "APEC.WSPublicos.sln"

# 3. Publica el proyecto de la API

WORKDIR "/app/APEC.WSPublicos.API" 

RUN ls -al

# Publicamos directamente a la carpeta raíz /publish.
RUN dotnet publish -c Release -o /publish /p:UseAppHost=false

# ----------------------------------------------------
# STAGE 2: Final (Runtime)
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos todos los archivos publicados desde la ruta /publish a la carpeta /app
COPY --from=build /publish .

# Configuración del puerto 9880
ENV ASPNETCORE_URLS=http://+:9880
EXPOSE 9880

RUN ls

# Define el punto de entrada para iniciar la aplicación
ENTRYPOINT ["dotnet", "APEC.WSPublicos.API.dll"]