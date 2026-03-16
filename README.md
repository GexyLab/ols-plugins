# ols-plugins
Repository for community plugins of OpenLab Studio and OpenLab Standalone

# ols-plugin-project-directory
This plugin loads OpenLab Studio projects from a directory. 
The directory is specified in the OpenLab Studio config.json file. By default, the "projects" directory is created in the main OpenLab Studio directory.

## Config
Below is an example of the configuration for this plugin in the OpenLab Studio config.json file.
```json
[...]
"plugins": {
  "loadOnStart": true,
  "plugins": [
    {
      "enabled": true,
      "path": "OpenLab Studio\\plugins\\ols-plugin-projects-directory.dll",
      "config": {
        "projectsDir": "OpenLab Studio\\projects"
      }
    }
  ]
[...]
},
[...]
```

### Config parameters
Below are the plugin specific configuration parameters (config json object).
| Name  | Optional | 
| ------------- | ------------- |
| projectsDir  | No |
