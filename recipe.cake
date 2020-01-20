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
                            shouldRunDotNetCorePack: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server.Tests/*.cs",
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server/**/*.AssemblyInfo.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();