## DATABASES

* create database development enviroment

```
docker compose up -d
```

* if your system doesn't have a docker compose installed

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer2022!" `
  -p 1433:1433 `
  --name sqlserver2022 `
  --hostname sqlserver2022 `
   -v "$(pwd)/mssql-data:/var/opt/mssql/data" `
   -d `
   mcr.microsoft.com/mssql/server:2022-latest
```