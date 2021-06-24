FROM mcr.microsoft.com/dotnet/core/sdk:3.0
ENV PATH $PATH:/root/.dotnet/tools

COPY . /app
WORKDIR /app

RUN dotnet restore

RUN dotnet tool install --global dotnet-ef --version 3.0.0
RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.0.0
RUN dotnet-ef migrations add InitialCreate
RUN dotnet-ef database update

RUN dotnet build

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker

ENTRYPOINT dotnet run