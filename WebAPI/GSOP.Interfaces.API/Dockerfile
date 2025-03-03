#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WebAPI/GSOP.Interfaces.API/GSOP.Interfaces.API.csproj"
RUN dotnet build "WebAPI/GSOP.Interfaces.API/GSOP.Interfaces.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI/GSOP.Interfaces.API/GSOP.Interfaces.API.csproj" -c Release -o /app/publish

FROM node:23.1 AS node
WORKDIR /src
COPY . .
WORKDIR "/src/WebUI"
RUN yarn install && yarn build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node /src/WebUI/build wwwroot/UI
ENTRYPOINT ["dotnet", "GSOP.Interfaces.API.dll"]