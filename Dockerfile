# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só os arquivos de projeto primeiro, para o cache do Docker reaproveitar o restore
# entre builds quando apenas o código-fonte muda (não as dependências).
COPY CrmImobiliaria.slnx ./
COPY CrmImobiliaria.Domain/CrmImobiliaria.Domain.csproj CrmImobiliaria.Domain/
COPY CrmImobiliaria.Application/CrmImobiliaria.Application.csproj CrmImobiliaria.Application/
COPY CrmImobiliaria.Infrastructure/CrmImobiliaria.Infrastructure.csproj CrmImobiliaria.Infrastructure/
COPY CrmImobiliaria.Shared/CrmImobiliaria.Shared.csproj CrmImobiliaria.Shared/
COPY CrmImobiliaria.Web/CrmImobiliaria.Web.csproj CrmImobiliaria.Web/
COPY tests/CrmImobiliaria.Domain.Tests/CrmImobiliaria.Domain.Tests.csproj tests/CrmImobiliaria.Domain.Tests/

RUN dotnet restore CrmImobiliaria.Web/CrmImobiliaria.Web.csproj

COPY . .
RUN dotnet publish CrmImobiliaria.Web/CrmImobiliaria.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Roda como usuário não-root
RUN useradd --uid 1654 --user-group --create-home crmapp
USER crmapp

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CrmImobiliaria.Web.dll"]
