# ----------------------------------------------------
# STAGE 1: Build 
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

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
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-noble-chiseled AS final
WORKDIR /app

COPY --from=build /publish .

ENV ASPNETCORE_URLS=http://+:9880

EXPOSE 9880
ENTRYPOINT ["./APEC.WSPublicos.API"]