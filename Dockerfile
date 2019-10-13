FROM mcr.microsoft.com/dotnet/core/runtime:2.2
COPY /bin/Release/netcoreapp2.2/linux-x64/publish/ /app
WORKDIR /app

EXPOSE 80
EXPOSE 443
EXPOSE 4222

ENTRYPOINT ["dotnet", "Iot.Gateway.Arduino.dll"]