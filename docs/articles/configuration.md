# Configuration

## VPEAR.Server
The server configuration can be found in the *appsettings.josn*.

### Kestrel (Web Server)
The allowed hosts property is mandatory. It is very important to set the secret property, because all access tokens for the web api will be generated with this secret. The rest is the Kestrel endpoint configuration you can read more about [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-5.0#:~:text=%20Configure%20endpoints%20for%20the%20ASP.NET%20Core%20Kestrel,IConfiguration%20as%20input.%203%20ConfigureHttpsDefaults%20%28Action%3CHttpsConnectionAdapterOptions%3E%29.%20More%20).

```json
{
  ...
  "AllowedHosts": "*",
  "Secret": "your_random_secret",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:8080"
      }
    }
  },
  ...
}
```

### MariaDB (DB)
This is the database connection configuration. Replace the user and password parts with your own user name and password. Also change the version to your MariaDB version.

```json
{
  ...
  "MariaDb": {
    "Connection": "server=localhost;user=user_name;password=user_password;database=VPEARDbContext",
    "Version": "10.5"
  },
  ...
}
```

### Serilog (Logging)
This is the logging configuration. For more information look in to the [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration/) documentation. Currently the server uses only the Console and File Sinks.

```json
{
  ...
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "../../logs/log_.txt",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true
        }
      }
    ],
    "Enrich": [ "FromLogContext" ],
    "Properties": {
      "Application": "VPEAR.Server"
    }
  }
  ...
}
```

### Full
```json
{
  "AllowedHosts": "*",
  "Secret": "your_random_secret",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://domain.tld"
      }
    }
  },
  "MariaDb": {
    "MariaDb": {
    "Connection": "server=localhost;user=user_name;password=user_password;database=VPEARDbContext",
    "Version": "10.5"
  },
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "../../logs/log_.txt",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true
        }
      }
    ],
    "Enrich": [ "FromLogContext" ],
    "Properties": {
      "Application": "VPEAR.Server"
    }
  }
}
```

## VPEAR.Client
The client is build options can be controlled from the Unity IDE build settings.
