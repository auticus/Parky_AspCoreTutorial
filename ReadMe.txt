https://medium.com/agilix/docker-express-running-a-local-sql-server-express-204890cff699
This project utilizes a sql express database.  To do: dockerize this instance with some data already preconfigured
* - nuget packages = EntityFramework Core, and Entity Framework Core SQLServer

* - when running set up, within PackageManager Console - the command "add-migration AddNationalParkToDb" was run
---> this requires Microsoft.EntityFrameworkCore.Tools package to be installed from Nuget
---> this will create a Migrations folder in the solution which will create the tables needed (this is then moved into the Data folder for organization)

* - to update the database, ensure that the connection string in the appsettings.json file is correct and in the Package Manager Console run the command
---> "update-database"
---> this will create the schema structure in the sql server for the new database