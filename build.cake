#addin nuget:?package=Cake.Figlet&version=1.1.0
#tool "nuget:?package=GitVersion.CommandLine&version=5.0.0-beta2-61"
#addin "nuget:?package=Cake.Coverlet&version=2.5.4"

var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");
var solutionDir = System.IO.Directory.GetCurrentDirectory ();
var testResultDir = Argument ("testResultDir", System.IO.Path.Combine (solutionDir, "test-results")); // ./build.sh --target Build-Container -testResultsDir="somedir"
var artifactDir = Argument ("artifactDir", "./artifacts"); // ./build.sh --target Build-Container -artifactDir="somedir"
var buildNumber = Argument<int> ("buildNumber", 0); // ./build.sh --target Build-Container -buildNumber=5
var slnName = Argument ("slnName", "dotnet_googletrendtopics");
string versionInfo = null;

Information (Figlet ("dotnet-google-trend-topics"));
Task ("Clean")
	.Does (() => {

		var settings = new DeleteDirectorySettings {
			Recursive = true,
				Force = true
		};
		if (DirectoryExists (testResultDir)) {
			CleanDirectory (testResultDir);
			DeleteDirectory (testResultDir, settings);
		}

		if (DirectoryExists (artifactDir)) {
			CleanDirectory (artifactDir);
			DeleteDirectory (artifactDir, settings);
		}

		var binDirs = GetDirectories ("./**/bin");
		var objDirs = GetDirectories ("./**/obj");
		var testResDirs = GetDirectories ("./**/TestResults");
		CleanDirectories (binDirs);
		CleanDirectories (objDirs);
		CleanDirectories (testResDirs);
		DeleteDirectories (binDirs, settings);
		DeleteDirectories (objDirs, settings);
		DeleteDirectories (testResDirs, settings);
	    DotNetCoreClean(".");

	});



Task ("PrepareDirectories")
	.Does (() => {
		EnsureDirectoryExists (testResultDir);
		EnsureDirectoryExists (artifactDir);
	});

Task ("Restore")
	.Does (() => {
		DotNetCoreRestore ("./dotnet-google-trend-topics.sln");
	});
Task ("Build")
	.IsDependentOn ("Clean")
	.IsDependentOn ("PrepareDirectories")
	.IsDependentOn ("Restore")
	.Does (() => {

		var solution = GetFiles ("./*.sln").ElementAt (0);
		Information ("Build solution: {0}", solution);
		var settings = new DotNetCoreBuildSettings {
			Configuration = configuration
		};
		DotNetCoreBuild (solution.FullPath, settings);
	});
Task("Run-Tests")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild=true
    }; 
    
    var coverletSettings = new CoverletSettings();
    coverletSettings.CollectCoverage =true;
    coverletSettings.CoverletOutputFormat= CoverletOutputFormat.opencover;
    coverletSettings.CoverletOutputDirectory = "./coverlet";
    var files = GetFiles("./test/**/*.csproj");
    Information($"Found {files.Count} test project! ");

    var reportFileName = string.Empty;
    foreach(var file in files){
        reportFileName= $"results-{file.GetFilenameWithoutExtension()}.xml";
        Information($"Generate report file {reportFileName}");
        coverletSettings.CoverletOutputName = reportFileName;
        DotNetCoreTest(file.FullPath,settings,coverletSettings);
    }
});

Task ("Pack")
	.Does (() => {
		var projectPath = "./src/googletrendstopics-tool/googletrendstopics-tool.csproj";
		var settings = new DotNetCorePackSettings {
			Configuration = configuration,
			OutputDirectory = artifactDir,
			NoBuild =true,
			ArgumentCustomization = args => {				
				
				return args;
			}
		};
		DotNetCorePack(projectPath, settings);
	});

Task("Push")

.Does(()=>{
	var apiKey=	EnvironmentVariable("nugetKey");
	 var settings = new DotNetCoreNuGetPushSettings
     {
         Source = "https://www.myget.org/F/ilkerhalil/api/v3/index.json",
         ApiKey =apiKey,
     };
	 
	 var path=  GetFiles ("./artifacts/Google.TrendTopic.Dotnet.Cli.*.nupkg").First ();
	
     DotNetCoreNuGetPush(path.FullPath, settings);
});

FilePathCollection GetSrcProjectFiles () {

	return GetFiles ("./src/**/*.csproj");
}

FilePathCollection GetTestProjectFiles () {

	return GetFiles ("./test/**/*Test/*.csproj");
}

FilePath GetSlnFile () {
	return GetFiles ("./**/*.sln").First ();
}

FilePath GetMainProjectFile () {
	foreach (var project in GetSrcProjectFiles ()) {
		if (project.FullPath.EndsWith ($"googletrendstopics-tool")) {
			return project;
		}
	}
	Error ("Cannot find main project file");
	return null;
}

RunTarget (target);
