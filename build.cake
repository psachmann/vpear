// Tools/Addins
// #tool nuget:?package=ReportGenerator&version=4.8.0
// #addin nuget:?package=Cake.Codecov&version=0.9.1
// #addin nuget:?package=Cake.DocFx&version=0.13.1
// #addin nuget:?package=Cake.DotNetCoreEf&version=0.10.0
#addin nuget:?package=Cake.Figlet&version=1.3.1

// Arguments
var target = Argument("target", "Test");
var config = Argument("config", "Debug");

// // Paths
// var rootDir = "./";

// Setup/Teardown
Setup(context =>
{
    Information(Figlet($"Starting {target}"));
});

Teardown(context =>
{
    Information(Figlet($"Finished {target}"));
});

// Tasks
Task("Clean")
    .Does(() =>
{
    Information(Figlet("Cleaning directories..."));
});

Task("Build")
    .Does(() =>
{
    Information("Building...");
});


Task("Run")
    // TODO: some more dependencies
    .IsDependentOn("Build")
    .Does(() =>
{
    // TODO: run the server
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Testing...");
});

Task("Docs")
    .Does(() =>
{
    // TODO: generate docs
});

Task("DB")
    .Does(() =>
{
    // TODO: prepate db for the server
});

RunTarget(target);
