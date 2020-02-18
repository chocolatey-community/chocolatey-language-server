#load nuget:?package=Cake.Recipe&version=1.1.1

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "chocolatey-language-server",
                            repositoryOwner: "chocolatey-community",
                            repositoryName: "chocolatey-language-server",
                            appVeyorAccountName: "chocolateycommunity",
                            shouldRunDupFinder: false, // Unable to run dupFinder due to not finding some kind of external annotations file
                            shouldRunInspectCode: false, // Same reason as dupe finder
                            shouldRunCodecov: true,
                            shouldRunGitVersion: true,
                            shouldRunDotNetCorePack: true,
                            shouldDeployGraphDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server.Tests/*.cs",
                            BuildParameters.RootDirectoryPath + "/Source/chocolatey-language-server/**/*.AssemblyInfo.cs" },
                            testCoverageFilter: "+[chocolatey-language-server]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();