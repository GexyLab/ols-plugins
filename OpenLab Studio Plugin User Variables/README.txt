OpenLab Studio Config
---------------------
{
    "enabled": true,
    "path": "OpenLab Studio\\plugins\\OpenLabStudio-Plugin-User-Variables.dll",
    "config": {
        "envVars": true, 
        "builtinVars": true,
        "specialFoldersVars": true,
        "userVars" : true,
        "path": "OpenLab Studio\\config\\OpenLabStudio-Plugin-User-Variables.json"
    }
}

The envVars, builtinVars, specialFoldersVar and userVars is optionally,  envVars, builtinVars, specialFoldersVar default are false, and userVars default is true

OpenLabStudio-Plugin-User-Variables.json
----------------------------------------
"dictionary": {
        "var-name": "var-value",
}


Use
---
In my string build with Text class use variable like this

"hi, i'm a %%var-name
