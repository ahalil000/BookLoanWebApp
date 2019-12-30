FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY BookLoan.csproj ./
RUN dotnet restore ./BookLoan.csproj
COPY . .
WORKDIR /src/
RUN dotnet build BookLoan.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BookLoan.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookLoan.dll"]
