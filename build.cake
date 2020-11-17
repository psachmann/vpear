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
var htmlDocs = "./html";
var testResults = "./tests/**/TestResults";
var testSettings = "./tests/Coverlet.runsettings";

// Tasks
Task("Clean")
    .Does(() =>
    {
        CleanDirectories(GetDirectories(htmlDocs));
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
    // TODO: some more dependencies
    .IsDependentOn("Build")
    .Does(() =>
    {
        // TODO: run the server
    });

Task("Docs")
    .Does(() =>
    {
        // TODO: generate docs
    });

Task("DB")
    .Does(() =>
    {
        // TODO: prepare db for the server
    });

RunTarget(target);
