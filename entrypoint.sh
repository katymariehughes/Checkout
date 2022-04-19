#!/bin/bash

# Run init-script with long timeout - and make it run in the background
/opt/mssql-tools/bin/sqlcmd -S localhost -l 60 -U SA
 \ -P "MyPass@word" -i init.sql &
# Start SQL server
/opt/mssql/bin/sqlservr