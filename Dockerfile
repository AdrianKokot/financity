FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM node:alpine AS front
WORKDIR /app
COPY client .
RUN yarn && yarn build

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["api/Financity.Presentation/Financity.Presentation.csproj", "Financity.Presentation/"]
COPY ["api/Financity.Infrastructure/Financity.Infrastructure.csproj", "Financity.Infrastructure/"]
COPY ["api/Financity.Persistence/Financity.Persistence.csproj", "Financity.Persistence/"]
COPY ["api/Financity.Application/Financity.Application.csproj", "Financity.Application/"]
COPY ["api/Financity.Domain/Financity.Domain.csproj", "Financity.Domain/"]
RUN dotnet restore "Financity.Presentation/Financity.Presentation.csproj"
COPY api/ .
COPY --from=front /app/dist/financity /app/build/wwwroot
WORKDIR "/src/Financity.Presentation"
RUN dotnet build "Financity.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Financity.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /src
COPY "api/Financity.Presentation/Resources" .
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=front /app/dist/financity wwwroot
COPY ["api/Financity.Presentation/Resources", "Resources"]
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Financity.Presentation.dll"]
