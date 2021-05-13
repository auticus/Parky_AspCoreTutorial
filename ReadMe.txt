https://medium.com/agilix/docker-express-running-a-local-sql-server-express-204890cff699
This project utilizes a sql express database.  To do: dockerize this instance with some data already preconfigured
* - nuget packages = EntityFramework Core, and Entity Framework Core SQLServer

* - when running set up, within PackageManager Console - the command "add-migration AddNationalParkToDb" was run
---> this requires Microsoft.EntityFrameworkCore.Tools package to be installed from Nuget
---> this will create a Migrations folder in the solution which will create the tables needed (this is then moved into the Data folder for organization)

* - to update the database, ensure that the connection string in the appsettings.json file is correct and in the Package Manager Console run the command
---> "update-database"
---> this will create the schema structure in the sql server for the new database

POPULATING DATA
Database was populated manually using SQL Management Studio

SWASHBUCKLE.ASPNET.CORE
The swagger utility used for testing and documenting the API

Within launchsettings.json make sure the launchUrl is removed (otherwise it will try to run the weatherforecast controller which was also removed)
The startup config is setup to pull the swagger json file in as the endpoint and you should see the swagger UI pop up when running the solution

To enable your xml documents in swagger you must go to project properties -> build - and turn on your XML Documentation File.  Also got rid of the absolute path
and just made it "ParkyAPI.xml"

After this the startup bootstrapper is modified to find the xml comment file using reflection

XML COMMENT SUPPRESSION
Explicitly in Build->Suppress Warnings added 1591 so that the missing xml comments aren't cluttering our warnings up

TRAILS
to add trails, we will add a new migration in nuget console  "add-migration addTrailsToDb"
Then "update-database" to push