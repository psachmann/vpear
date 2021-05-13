// Tools/Addins
#tool nuget:?package=ReportGenerator&version=4.8.8
#addin nuget:?package=Cake.DocFx&version=1.0.0
#addin nuget:?package=Cake.DotNetCoreEf&version=0.10.0
#addin nuget:?package=Cake.Figlet&version=2.0.0

// Arguments
var target = Argument("target", "Test");
var config = Argument("config", "Debug");
var name = Argument("name", "Init");


// Paths
var project = "./VPEAR.sln";
var htmlDocs = "./docs/vpear-docs";
var docfxSettings = "./docs/docfx.json";
var testResults = "./tests/**/TestResults";
var testSettings = "./tests/Coverlet.runsettings";
var reportFiles = "./tests/**/coverage.opencover.xml";
var reportDir = "./docs/vpear-docs/report";
var historyDir = "./docs/history";
var srcDir = "./src";
var server = "./src/VPEAR.Server/VPEAR.Server.csproj";

// Tasks
Task("Clean")
    .Does(() =>
    {
        CleanDirectories(GetDirectories(testResults));
    });

Task("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings()
        {
            Configuration = config,
            Verbosity = DotNetCoreVerbosity.Minimal,
        };

        DotNetCoreBuild(project, settings);
    })
    .OnError(() =>
    {
        Information(Figlet("Build  failed"));
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings()
        {
            Configuration = config,
            NoBuild = true,
            NoRestore = true,
            Settings = File(testSettings),
            Verbosity = DotNetCoreVerbosity.Minimal,
        };

        DotNetCoreTest(project, settings);
    })
    .OnError(() =>
    {
        Information(Figlet("Test failed"));
    });

Task("Run")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreRunSettings()
        {
            Configuration = config,
            NoBuild = true,
            NoRestore = true,
            Verbosity = DotNetCoreVerbosity.Minimal,
        };

        DotNetCoreRun(server, null, settings);
    });

Task("Docs")
    .IsDependentOn("Clean")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var settings = new ReportGeneratorSettings()
        {
            AssemblyFilters = new[] { "+VPEAR.*" },
            HistoryDirectory = Directory(historyDir),
            ReportTypes = new[] { ReportGeneratorReportType.Html, ReportGeneratorReportType.Badges },
            Verbosity = ReportGeneratorVerbosity.Info,
        };

        ReportGenerator(new GlobPattern(reportFiles), Directory(reportDir), settings);
        DocFxMetadata(File(docfxSettings));
        DocFxBuild(File(docfxSettings));
    });

Task("Migration-Add")
    .Does(() =>
    {
        var settings = new DotNetCoreEfMigrationAddSettings()
        {
            Configuration = config,
            OutputDir = "Data/Migrations",
            Migration = name,
            Project = "VPEAR.Server/VPEAR.Server.csproj",
        };

        DotNetCoreEfMigrationAdd(srcDir, settings);
    });

Task("Database-Update")
    .Does(() =>
    {
        var settings = new DotNetCoreEfDatabaseUpdateSettings()
        {
            Configuration = config,
            Migration = name,
            Project = "VPEAR.Server/VPEAR.Server.csproj",
        };

        DotNetCoreEfDatabaseUpdate(srcDir, settings);
    });

RunTarget(target);
