# ols-plugin-project-directory
This plugin loads OpenLab Studio projects from a directory. 
The directory is specified in the OpenLab Studio config.json file. By default, the "projects" directory is created in the main OpenLab Studio directory.

## Config
Below is an example of the configuration for this plugin in the OpenLab Studio config.json file.
```
[...]
"plugins": {
  "loadOnStart": true,
  "plugins": [
    {
      "enabled": true,
      "path": "OpenLab Studio\\plugins\\ols-plugin-projects-directory.dll",
      "config": {
        "projectsDir": "projects"
      }
    }
  ]
[...]
},
[...]
```

### Config parameters
Below are the plugin specific configuration parameters (config json object).
| Name  | Required | 
| ------------- | ------------- |
| projectsDir  | Yes |

#### Parameter: projectsDir
This parameter specifies the root directory of the projects. It can be a complete path or a relative path to the OpenLab Studio root directory, like the config exemple reported above. 

Relative path exemple
```
"projectsDir": "projects"
```

Absolute path exemple
```
"projectsDir": "C:\\Users\\User\\ols\\projects"
```
