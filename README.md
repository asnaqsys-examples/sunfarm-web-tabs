# Sourcefiles for ASNA Monarch® Base SunFarm Example

  Click [Here to read: 10 Basic UI Enhancements](https://asnaqsys.github.io/examples/sunfarm/sunfarm.html) Example's documentation.

This repository contains two Visual Studio 2022 Solutions:

| Solution name | Application Logic programming languaje |
| :- | :- |
| SunFarmSolution.sln | ASNA Encore® RPG |
| SunFarmCSharpSolution.sln | C# |

Each Solution contains two projects:

1. The Application Logic project (implemented in the programming logic indicated above).
2. The ASP.NET Core Webiste.

>Note: The two implementations execute the same logic.

Once you have decided which solution to open, adjust the Website configuration to setup which of the two logic implementations to use.

Locate the configuration file: `~\SunFarmSite\appsettings.json`

Adjust the `AssemblyList` array includes both pathnames to the two implementation's .Net assemblies. The order in the list determines which to execute. Set the one you want to execute as the first item in the array.

```json
{
    "MonaServer": {
        "AssemblyList": [
        "C:/Projects/sunfarm-encore/SunFarmLogic_CS/bin/Debug/net6.0/ACME.SunFarmCustomers.dll",
        "C:/Projects/sunfarm-encore/SunFarmLogic/bin/Debug/net6.0/ACME.SunFarmCustomers.dll"
        ]
    }
}
```




