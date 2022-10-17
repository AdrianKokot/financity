FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:alpine AS front
WORKDIR /app
COPY client .
RUN yarn && yarn build

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
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
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=front /app/dist/financity wwwroot
ENTRYPOINT ["dotnet", "Financity.Presentation.dll"]
