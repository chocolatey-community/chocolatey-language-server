#load nuget:?package=Cake.Recipe&version=1.1.1

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "chocolatey-language-server",
                            repositoryOwner: "chocolatey-community",
                            repositoryName: "chocolatey-language-server",
                            appVeyorAccountName: "chocolateycommunity",
                            shouldRunGitVersion: true,
                            shouldRunDotNetCorePack: true,
                            shouldDeployGraphDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server.Tests/*.cs",
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server/**/*.AssemblyInfo.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

var NativeRuntimes = new Dictionary<PlatformFamily, string>
{
    [PlatformFamily.Windows] = "win-x64",
    [PlatformFamily.Linux]   = "linux-x64",
    [PlatformFamily.OSX]     = "osx-x64",
};

Task("DotNetCore-Publish")
    .IsDependeeOf("Create-NuGet-Packages")
    .IsDependentOn("DotNetCore-Test")
    .Does(() =>
{
    foreach(var runtime in NativeRuntimes)
    {
        var runtimeName = runtime.Value;

        var settings = new DotNetCorePublishSettings
        {
            Framework = "netcoreapp3.1",
            Runtime = runtimeName,
            NoRestore = false,
            Configuration = BuildParameters.Configuration,
            OutputDirectory = BuildParameters.Paths.Directories.TempBuild.Combine("Native").Combine(runtimeName),
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", BuildParameters.Version.SemVersion)
                            .WithProperty("AssemblyVersion", BuildParameters.Version.Version)
                            .WithProperty("FileVersion",  BuildParameters.Version.Version)
                            .WithProperty("AssemblyInformationalVersion", BuildParameters.Version.InformationalVersion)
        };

        settings.ArgumentCustomization =
            arg => arg
            .Append("/p:PublishTrimmed=true");

        DotNetCorePublish(BuildParameters.SolutionFilePath.FullPath, settings);
    }
});



Build.RunDotNetCore();