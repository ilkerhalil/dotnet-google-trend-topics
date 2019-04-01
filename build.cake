#addin nuget:?package=Cake.Figlet&version=1.1.0
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0-beta0012"

var target = Argument ("target", "Default");
var solutionDir = System.IO.Directory.GetCurrentDirectory ();

var testResultDir = Argument ("testResultDir", System.IO.Path.Combine (solutionDir, "test-results")); // ./build.sh --target Build-Container -testResultsDir="somedir"
var artifactDir = Argument ("artifactDir", "./artifacts"); // ./build.sh --target Build-Container -artifactDir="somedir"
var buildNumber = Argument<int> ("buildNumber", 0); // ./build.sh --target Build-Container -buildNumber=5
var dockerRegistry = Argument ("dockerRegistry", "local"); // ./build.sh --target Build-Container -dockerRegistry="local"
var slnName = Argument ("slnName", "dotnet_googletrendtopics");
string versionInfo = null;

Information (Figlet ("dotnet-google-trend-topics"));

Task ("Calculate-Version")
	.Does (() => {
		var settings = new GitVersionSettings();
		settings.NoFetch = true;
		settings.OutputType= GitVersionOutput.Json;
		var gitVersion = GitVersion(settings);
	    versionInfo = gitVersion.NuGetVersionV2;
		Information ($"Version  {versionInfo}");

	});

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
	});

Task ("PrepareDirectories")
	.Does (() => {
		EnsureDirectoryExists (testResultDir);
		EnsureDirectoryExists (artifactDir);
	});

Task ("Restore")
	.Does (() => {
		DotNetCoreRestore ();
	});

Task ("Build")
	.IsDependentOn ("Clean")
	.IsDependentOn ("PrepareDirectories")
	.IsDependentOn ("Restore")
	.Does (() => {
		var solution = GetFiles ("./*.sln").ElementAt (0);
		Information ("Build solution: {0}", solution);

		var settings = new DotNetCoreBuildSettings {
			Configuration = "Release"
		};

		DotNetCoreBuild (solution.FullPath, settings);
	});


Task ("Pack")
.IsDependentOn(("Calculate-Version"))
	.Does (() => {
	

		var projectPath = "./src/googletrendstopics-tool";
		var settings = new DotNetCorePackSettings {
			Configuration = "Release",
				OutputDirectory = artifactDir,ArgumentCustomization = (args) => {
                  args.Append("/p:Version={0}", versionInfo);
                  return args;

               }
		};
		DotNetCorePack (projectPath, settings);


	});



Task ("Default")
	.Does (() => {
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