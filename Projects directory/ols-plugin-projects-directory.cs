using OpenLabSDK.config;
using OpenLabSDK.error;
using OpenLabSDK.events;
using OpenLabSDK.plugin;
using OpenLabSDK.ui;

namespace OpenLabStudio.plugins
{
    public class OLSPluginProjectsDirectory : Plugin
    {
        internal class Info : IPluginInfo
        {
            public string Name { get; } = "ols-plugin-projects-directory";
            public string Title { get; } = "OpenLab Studio projects directory";
            public int[] version { get; } = { 1, 0, 0 };
            public string vendorUID { get; }
            public string url { get; } = "https://github.com/GexyLab/ols-plugins/tree/main";
            public string description { get; } = "Load OpenLab Studio projects from directory";
        }

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
            log.info("Deinit plugin" + pluginInfo.Title);
            return 0;
        }

        public override int init()
        {
            log.info("Init plugin " + pluginInfo.Title);

            eventsManager.addEventHandler("ols.ready", (object sender, EventArgs e) =>
            {
                log.info($"Plugin: {pluginInfo.Name} hello!!");
                return true;
            });

            return 0;
        }
    }
}
