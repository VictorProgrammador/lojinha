# Define a imagem base
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Define o diretório de trabalho
WORKDIR /app

# Copia o arquivo do projeto e restaura as dependências
COPY . ./
RUN dotnet restore

# Publica o projeto
RUN dotnet publish -c Release -o out

# Define a imagem base do runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

# Define a porta que a aplicação irá escutar
EXPOSE 5000

# Inicia o aplicativo
ENTRYPOINT ["dotnet", "meuapp.dll"]
