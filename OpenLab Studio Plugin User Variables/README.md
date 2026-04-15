# OpenLab Studio Plugin User Variables
This plugin allows the user to create custom variables. Variables can be used in the fields where they are allowed. This plugin also provides all environment variables, special directories, and some variables from the Environment class.

It also provides some LogicBlocks for using variables in the Desk Editor.

# Config
Is possible to enable/disable some group of variables. The user variables must be defined in the "dictionary" object. Each property of "dictionary" object represent a variable, the key is the name and the value is the value of variable. Following an exemple:

```
{
    "enabled": true,
    "path": "OpenLab Studio\\plugins\\OpenLabStudio-Plugin-User-Variables.dll",
    "config": {
        "envVars": true, 
        "builtinVars": true,
        "specialFoldersVars": true,
        "userVars" : true,
        "dictionary": {
            "var-name": "var-value",
        }
    }
}
```

## Properties
|Name|Optional|Default|Description|
|:----|:--------:|:-------:|:-----------|
|envVar|yes|false|Enable/disable windows(user and system) variables|
|builtinVar|yes|false|Enable/disable builtin variables(ex.: "processPath", see System.Evironment class)|
|specialFoldersVars|yes|false|Enable/disable special folders variables(ex.: MyDocuments path)|
|userVars|yes|true|Enable/disable user defined variables(defined in the "dictionary" object|

# Use
The variables is identify by the %% symbol at the beginning of variable name, like this
```
"hi, i'm a %%var-name
```

## Alias
It's possible to use variables inside the others variables, to create the aliases. Un exemple:

```
"dictionary": {
     "workDir": "%%processPath"
}
```

## Predefined variables
All variable of System.Environment.SpecialFolder class have been added to predefined variables, see https://learn.microsoft.com/it-it/dotnet/api/system.environment.specialfolder?view=net-7.0 to complete list, for the System.Environment class have been added only these variables(see https://learn.microsoft.com/it-it/dotnet/api/system.environment?view=net-8.0), these variables as inetnded as builtin variables

|Name|
|:----|
|CommandLine|
|CurrentDirectory|
|CurrentManagedThreadId|
|Is64BitOperatingSystem|
|Is64BitProcess|
|IsPrivilegedProcess|
|machineName|
|NewLine|
|ProcessId|
|ProcessorCount|
|ProcessPath|
|SystemDirectory|
|UserDomainName|
|useUserInteractiverName|
|UseWorkingSetrName|

Other builtin variables

|Name|Description|
|:----|:--------|
|||
