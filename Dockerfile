# ----------------------------------------------------
# STAGE 1: Build 
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. Copia el archivo de la solución y todo el código fuente.
# Esto incluye las carpetas de Clean Architecture.
COPY ["APEC.WSPublicos.sln", "."]
COPY . .

# 2. Restaura las dependencias
RUN dotnet restore "APEC.WSPublicos.sln"

# 3. Publica el proyecto de la API
WORKDIR "/app/APEC.WSPublicos.API" 

# Publicamos a una ruta absoluta (/publish)
RUN dotnet publish -c Release -o /publish /p:UseAppHost=false

# 4. === COPIA CLAVE: Copiar la base de datos ===
# Volvemos a la raíz del contexto (/app)
WORKDIR /app
# *** RUTA CORREGIDA ***: La DB se copia desde la carpeta del proyecto API.
COPY APEC.WSPublicos.API/AppDbContextSqlite.db /publish/ 

# === PUNTO DE DEPURACIÓN 1: Verificar el resultado de la publicación ===
RUN echo "--- Contenido del directorio /publish (Etapa Build) ---"
RUN ls -al /publish
# ----------------------------------------------------------------------


# ----------------------------------------------------
# STAGE 2: Final (Runtime)
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiamos todos los archivos publicados, incluyendo la DB y la DLL.
COPY --from=build /publish .

# === PUNTO DE DEPURACIÓN 2: Verificar la copia en la Etapa Final ===
RUN echo "--- Contenido del directorio /app (Etapa Final) ---"
RUN ls -al /app
# ------------------------------------------------------------------


# Configuración del puerto 9880
ENV ASPNETCORE_URLS=http://+:9880
EXPOSE 9880

# Define el punto de entrada para iniciar la aplicación
ENTRYPOINT ["dotnet", "--info"]