# ----------------------------------------------------
# STAGE 1: Build 
# Usamos el SDK para compilar y publicar
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copia el archivo de la solución y restaura las dependencias
COPY ["APEC.WSPublicos.sln", "."]
COPY . .

RUN dotnet restore "APEC.WSPublicos.sln"

# 2. Publica el proyecto de la API
WORKDIR "/app/APEC.WSPublicos.API" 

# Publicamos la aplicación en la carpeta 'out'
RUN dotnet publish -c Release -o out /p:UseAppHost=false

# ----------------------------------------------------
# STAGE 2: Final (Runtime)
# Usamos la imagen de ejecución, más pequeña y segura
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 1. Copia los artefactos publicados desde la etapa 'build'
COPY --from=build /app/APEC.WSPublicos.API/out .

# 2. Configuración y ejecución
# El puerto 8080 es el estándar en las nuevas imágenes base
ENV ASPNETCORE_URLS=http://+:9880
EXPOSE 9880

# 3. Define el punto de entrada para iniciar la aplicación
ENTRYPOINT ["dotnet", "APEC.WSPublicos.API.dll"]

# 4. Mensajes informativos (basados en tu esquema)
# NOTA: Estos mensajes solo se ven durante la construcción de la imagen.
RUN echo " -----------------------------------------------"
RUN echo " Application is running at http://localhost:9880"
RUN echo " -----------------------------------------------"