FROM mcr.microsoft.com/dotnet/sdk:8.0.401-1-alpine3.20 AS installer-env

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app/src/BpClockerAzureFunction && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
RUN apt-get update && \
    apt-get install -y tzdata && \
    ln -sf /usr/share/zoneinfo/Asia/Taipei /etc/localtime && \
    echo "Asia/Taipei" > /etc/timezone && \
    dpkg-reconfigure -f noninteractive tzdata
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    TZ="Asia/Taipei"

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]