# https://hub.docker.com/_/microsoft-dotnet
# FROM mcr.microsoft.com/windows/servercore:1809

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY GrpcService1/*.csproj ./GrpcService1/
COPY ConsoleApp1/. ./ConsoleApp1/
COPY MusicStreaming.Core/. ./MusicStreaming.Core/
COPY MusicStreaming.Player/. ./MusicStreaming.Player/
RUN dotnet restore

# copy everything else and build app
COPY GrpcService1/. ./GrpcService1/

WORKDIR /source/GrpcService1
RUN dotnet publish -c release -o /app --restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "GrpcService1.dll"]