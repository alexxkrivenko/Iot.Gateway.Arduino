FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY . /app
WORKDIR /app

EXPOSE 80
EXPOSE 443
EXPOSE 6222
EXPOSE 8222

ENTRYPOINT ["dotnet", "Iot.Gateway.Arduino.dll"]