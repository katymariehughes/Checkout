FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
#mcr.microsoft.com/azure-sql-edge
ENV ACCEPT_EULA y
ENV SA_PASSWORD MyPass@word
COPY ./init.sql .
COPY ./entrypoint.sh .
EXPOSE 1433
CMD /bin/bash ./entrypoint.sh