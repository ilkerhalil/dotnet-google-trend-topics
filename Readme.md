## Install

```
dotnet tool install -g Google.TrendTopic.Dotnet.Cli --add-source https://www.myget.org/F/ilkerhalil/api/v3/index.json
```
```
Usage: googletrendstopics-tool [options]
Options:
-g|--geo <GEO> Trends geo,Default value US
-?|-h|--help Show help information

googletrendstopics-tool --geo=TR
```
Build status
    
[![Build Status](https://dev.azure.com/ilkerhalil/dotnet-google-trend-topics/_apis/build/status/ilkerhalil.dotnet-google-trend-topics?branchName=development)](https://dev.azure.com/ilkerhalil/dotnet-google-trend-topics/_build/latest?definitionId=5&branchName=development)


| Package | MyGet Stable | 
| ------- | ------------ | 
| [Google.TrendTopic.Dotnet.Cli](https://www.myget.org/feed/ilkerhalil/package/nuget/Google.TrendTopic.Dotnet.Cli) | [![MyGet Badge](https://shields.io/myget/ilkerhalil/v/Google.TrendTopic.Dotnet.Cli)](https://www.myget.org/feed/ilkerhalil/package/nuget/Google.TrendTopic.Dotnet.Cli)

## How to build

dotnet-google-trend-topics is built against the latest DotNet Core 2.2.

-   [Install](https://www.microsoft.com/net/download/core#/current)  the latest .NET Core 3.1 SDK
-   Run  `./build.ps1 -Target Build`  or  `./build.sh --target=Build  in the root of the repo

## Acknowledgements
dotnet-google-trend-topics is built using the following great open source projects and free services:

* [.NET Core](https://github.com/dotnet/core)
* [MinVer](https://github.com/adamralph/minver)
* [McMaster.Extensions.CommandLineUtils](https://github.com/natemcmaster/CommandLineUtils)
* [Azure Pipelines
](https://azure.microsoft.com/tr-tr/services/devops/pipelines//)

