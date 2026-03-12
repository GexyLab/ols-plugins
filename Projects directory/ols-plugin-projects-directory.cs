using OpenLabSDK.config;
using OpenLabSDK.error;
using OpenLabSDK.events;
using OpenLabSDK.plugin;
using OpenLabSDK.ui;
using System.Windows;

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

        string projectsDir = "";

        #endregion

        public OLSPluginProjectsDirectory(
            IErrorManager _errorManager,
            IPluginsManager _pluginsManager,
            IEventsManager _eventsManager,
            IWindowsManager _windowsManager,
            PluginDefinition _pluginDefinition) : base(_errorManager, _pluginsManager, _eventsManager, _windowsManager, _pluginDefinition)
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

            

            eventsManager.addEventHandler("projectManager.existingsProjectsLoad.before", (object sender, EventArgs e) =>
            {
                try
                {
                    initProjectsDir();

                    MessageBox.Show(projectsDir);
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

        private void initProjectsDir()
        {
            if (pluginDefinition.haveConfig())
            {
                projectsDir = (string)pluginDefinition.config.Get("dir", "none");
            }
            else
            {
                string msg = "Plugin not have it's specific config, missing config json object in OpenLab Studio config file";
                log.error(msg);
                throw new OLSException(errorManager, msg);
            }
                
        }
    }
}
