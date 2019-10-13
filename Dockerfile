FROM mcr.microsoft.com/dotnet/core/runtime:2.2
COPY /app/bin/Release/netcoreapp2.2/linux-x64 /app
WORKDIR /app

EXPOSE 80
EXPOSE 443
EXPOSE 6222
EXPOSE 8222

ENTRYPOINT ["dotnet", "Iot.Gateway.Arduino.dll "]