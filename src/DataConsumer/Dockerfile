FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DataConsumer.csproj", "."]
RUN dotnet restore "./DataConsumer.csproj"
COPY . .
RUN dotnet publish "./DataConsumer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DataConsumer.dll"]