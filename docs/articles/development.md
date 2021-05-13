# Development

## Prerequisites
Some things have to be installed beforehand:

* Dotnet SDK
  * Install the dotnet sdk in version 5.0 from [here](https://dotnet.microsoft.com/download/dotnet/5.0)
  * `choco install dotnetsdk`
* Dotnet EF Core
  * Install the ef core command line tools
  * `dotnet tool install dotnet-ef -g`
* MariaDB
  * Install a local MariaDb Server in version 10.3 or newer
  * `choco install mariadb`
* Cake
  * You can install Cake as an global dotnet tool
  * `dotnet tool install --global Cake.Tool`
* Unity
  * If you want also build the Unity client, Unity 2020.3 LTS must als be installed
  * Use the [Unity Hub](https://unity3d.com/de/get-unity/download) to download the right version

## Build Commands
The project uses the Cake build tool for the server, if you want to build the client you have to use the Unity IDE. The following commands are supported via `dotnet cake --target=<command> options ...`:

| Command | Options | Description |
| - | - | - |
| clean | - | Cleans the output directories. Will be used be other commands. |
| build | --config=<Debug,Release> | Builds the project with the given config. Default config is Debug. |
| test | --config=<Debug,Release> | Runs the unit tests with the given config. Default config is Debug. |
| run | --config=<Debug,Release> | Runs the server with the given config. Default config is Debug. |
| docs | --config=<Debug,Release> | Creates the projects documentation. Default config is Debug. |
| migration-add | --config=<Debug,Release>, --name=<Name,> | Creates the migrations with the given name for the db. Default config is Debug. Only the Debug configuration has db seed data. The default name is Init. |
| database-update | --config=<Debug,Release>, --name=<Name,> | Applies migrations with the given name to the db. Default config is Debug. Only the Debug configuration has db seed data. The default name is Init. |

### Examples
Type in the folder with the *build.cake* file (should be project root).

* Run all unit test `dotnet cake --target=test`
* Run server in release mode `dotnet cake --target=run --config=Release`
* Create migration with seed data `dotnet cake --target=migration-add`
* Apply migration to db `dotnet cake --target=database-update`

## Run
The server and client have to started in different ways.

### VPEAR.Server
The steps to run the server:

* Make sure the prerequisites are fulfilled (Dotnet SDK, Dotnet EF Core, MariaDB, Cake)
* Make sure the you have the right configuration in your *appsettings.json*
* Create a migration `dotnet cake --target=migration-add`
* Apply the migration `dotnet cake --target=database-update`
* Run the server `dotnet cake --target=run`

### VPEAR.Client

Open the VPEAR.Client project with the Unity IDE and deploy the app to your android smartphone.
