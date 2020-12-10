// Tools/Addins
#tool nuget:?package=ReportGenerator&version=4.8.0
#addin nuget:?package=Cake.Codecov&version=0.9.1
#addin nuget:?package=Cake.DocFx&version=0.13.1
#addin nuget:?package=Cake.DotNetCoreEf&version=0.10.0
#addin nuget:?package=Cake.Figlet&version=1.3.1

// Arguments
var target = Argument("target", "Test");
var config = Argument("config", "Debug");

// Paths
var project = "./VPEAR.sln";
var htmlDocs = "./docs/vpear-docs";
var docfxSettings = "./docs/docfx.json";
var testResults = "./tests/**/TestResults";
var testSettings = "./tests/Coverlet.runsettings";
var reportFiles = "./tests/**/coverage.opencover.xml";
var reportDir = "./docs/vpear-docs/report";
var historyDir = "./docs/history";
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
            Verbosity = ReportGeneratorVerbosity.Error,
        };

        ReportGenerator(reportFiles, Directory(reportDir), settings);
        DocFxMetadata(File(docfxSettings));
        DocFxBuild(File(docfxSettings));
    });

Task("DB")
    .Does(() =>
    {
        // TODO: prepare db for the server
    });

RunTarget(target);
