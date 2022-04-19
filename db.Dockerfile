FROM mcr.microsoft.com/azure-sql-edge

ENV ACCEPT_EULA=1 \
    MSSQL_SA_PASSWORD=MyPass@word \
    MSSQL_PID=Developer \
    MSSQL_USER=SA

ADD schema.sql /docker-entrypoint-initdb.d

EXPOSE 1433