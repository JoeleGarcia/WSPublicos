# ----------------------------------------------------
# STAGE 1: Build 
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

RUN apt-get update \
    && apt-get install -y build-essential clang zlib1g-dev

COPY ["APEC.WSPublicos.sln", "."]

COPY . .

RUN dotnet restore "APEC.WSPublicos.sln"

WORKDIR "/app/APEC.WSPublicos.API" 

RUN dotnet publish "APEC.WSPublicos.API.csproj" \
    -r linux-x64 \
    -c Release \
    -p:PublishAot=true \
    --self-contained true \
    -o /publish

# ----------------------------------------------------
# STAGE 2
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-noble-chiseled AS final
WORKDIR /app

COPY --from=build /publish .

RUN chmod +x ./APEC.WSPublicos.API

ENV ASPNETCORE_URLS=http://+:9880

EXPOSE 9880
ENTRYPOINT ["./APEC.WSPublicos.API"]