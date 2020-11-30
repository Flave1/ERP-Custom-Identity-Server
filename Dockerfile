FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src  
COPY ["APIGateway/APIGateway.csproj", "APIGateway/"]
RUN dotnet restore "APIGateway/APIGateway.csproj"
COPY . .
WORKDIR "/src/APIGateway"
RUN dotnet build "APIGateway.csproj" -c Release -o /app/build 

FROM build as publish
RUN  dotnet publish "APIGateway.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGateway.dll"]