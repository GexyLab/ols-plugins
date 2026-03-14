using OpenLab.GeJSON;
using OpenLabSDK.config;
using OpenLabSDK.error;
using OpenLabSDK.events;
using OpenLabSDK.plugin;
using OpenLabSDK.ui;
using OpenLabStudio.project;
using System.IO;
using System.Windows;
using System.Windows.Shapes;

namespace OpenLabStudio.plugins
{
    public class OLSPluginProjectsDirectory : Plugin
    {
        #region Variables

        internal class Info : IPluginInfo
        {
            public string Name { get; } = "ols-plugin-projects-directory";
            public string Title { get; } = "OpenLab Studio projects directory";
            public int[] version { get; } = { 1, 0, 0 };
            public string vendorUID { get; }
            public string url { get; } = "https://github.com/GexyLab/ols-plugins/tree/main";
            public string description { get; } = "Load OpenLab Studio projects from directory";
        }

        string? projectsDirPath;
        DirectoryInfo projectsDir;
        List<Project> projects = new();


        #endregion

        public OLSPluginProjectsDirectory(
            IErrorManager _errorManager,
            IPluginsManager _pluginsManager,
            IEventsManager _eventsManager,
            IWindowsManager _windowsManager,
            ProjectsManager _projeProjectsManager,
            PluginDefinition _pluginDefinition) : base(_errorManager, _pluginsManager, _eventsManager, _windowsManager, _projeProjectsManager, _pluginDefinition)
        {
            pluginInfo = new Info();
        }

        public override int deinit()
        {
            log.info("Deinit plugin" + pluginInfo.Name);
            return 0;
        }

        public override int init()
        {
            log.info($"Init plugin {pluginInfo.Title} ({pluginInfo.Name})");

            eventsManager.addEventHandler("projectManager.projects.load", (object sender, EventArgs e) =>
            {
                try
                {
                    initProjectsDir();
                    readProjects();

                    foreach (var project in projects) {
                        ((ProjectsManager)sender).AddProject(project);
                    }

                }
                catch { throw; }
                

                
                return true;
            });

            /*eventsManager.addEventHandler("ols.ready", (object sender, EventArgs e) =>
            {
                log.info($"Plugin: {pluginInfo.Name} hello!!");
                return true;
            });*/

            return 0;
        }

        /*private string applyVariables(string path)
        {
            //  user dir
            var userDir = new DirectoryInfo(Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));

            return path.Replace("%userDir%",userDir))

        }*/

        /// <summary>
        /// Get existing projects root directory or create it if not exist
        /// </summary>
        /// <exception cref="OLSException"></exception>
        private void initProjectsDir()
        {
            if (pluginDefinition.haveConfig())
            {
                // Projects root dir
                try
                {
                    projectsDirPath = (string)pluginDefinition.config.Get("projectsDir", null);

                    // check or create projects root dir
                    if (!Directory.Exists(projectsDirPath))
                    {
                        projectsDir = Directory.CreateDirectory(projectsDirPath);
                    }
                    else
                    {
                        projectsDir = new DirectoryInfo(projectsDirPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new OLSException(errorManager, ex);
                }
            }
            else
            {
                throw new OLSException(errorManager, "Plugin not have it's specific config, missing \"config\" json object in OpenLab Studio config file");
            }
                
        }


        /// <summary>
        /// Read all projects in the projects root dir.  Each project is in a own directory and defined by project.json file 
        /// </summary>
        private void readProjects()
        {
            log.debug($"Reading projects from directory {projectsDirPath}");
            foreach (DirectoryInfo projectDir in projectsDir.GetDirectories())
            {
                log.debug($"Check project content({projectDir.FullName})");

                FileInfo projectFile = new FileInfo(projectDir.FullName+"\\project.json");
                if (projectFile.Exists)
                {
                    log.debug("Found project.json file, reading project definition");
                    JObject projectDef = readProjectMainFile(projectFile);

                    log.debug("Instatiate project");
                    Project prj = new Project(projectsManager, projectDef);
                    projects.Add(prj);
                }
                else
                {
                    log.error("file project.json not found, skip project");
                    continue;
                }


                    /*foreach (FileInfo file in projectDir.GetFiles())
                    {
                        if (file.Name == "project.json")
                        {
                            log.debug("Found project file(project.json)");
                        }
                    }*/
            }
        }

        /// <summary>
        /// Read the json main file of project(project.json)
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Return GeJson object with content of project.json</returns>
        private JObject readProjectMainFile(FileInfo projectFile)
        {
            string content = File.ReadAllText(projectFile.FullName);
            JObject projectDef = new JObject(content);

            return projectDef;
        }
    }
}
