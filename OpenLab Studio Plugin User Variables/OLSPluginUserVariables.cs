using OpenLab.GeJSON;
using OpenLabSDK.config;
using OpenLabSDK.error;
using OpenLabSDK.events;
using OpenLabSDK.plugin;
using OpenLabSDK.ui;
using OpenLabStudio.project;
using System.Windows;
using System.Xml.Linq;
using static OpenLabSDK.expression.Text;


namespace OpenLab_Studio_Plugin_User_Variables
{
    public class OLSPluginUserVariables : Plugin
    {
        internal class Info : IPluginInfo
        {
            public string Name { get; } = "ols-plugin-user-variables";
            public string Title { get; } = "OpenLab Studio User Variable";
            public int[] version { get; } = { 1, 0, 0 };
            public string vendorUID { get; }
            public string url { get; } = "https://github.com/GexyLab/ols-plugins/tree/main";
            public string description { get; } = "Create and use user defined, envvar and buildin variables";
        }

        #region variables

        /// <summary>
        /// Delegate to call when client call variable name
        /// </summary>
        /// <param name="value">the string to process in callback</param>
        /// <returns></returns>
        public delegate string BuiltinVariableCallback(string source);

        /// <summary>
        /// Dictionary of variables
        /// </summary>
        Dictionary<string, BuiltinVariableCallback> dictionary = new();

        /// <summary>
        /// Enable/disable buildin variables, default: false
        /// </summary>
        bool builtinEnabled = false;
        
        
        /// <summary>
        /// Enable/disable user & system environment variables, default: false
        /// </summary>
        bool envvarsEnabled = false;

        /// <summary>
        /// Enable/disable Windows special forlders variables(System.Environment.SpecialFolder), default: false
        /// </summary>
        bool specialFoldersEnabled = false;

        /// <summary>
        /// Enable/disable user variables define in the config file, default: true
        /// </summary>
        bool userEnabled = true;

        #endregion

        public OLSPluginUserVariables(IPluginsManager _pluginsManager, IEventsManager _eventsManager, IWindowsManager _windowsManager, ProjectsManager _projectsManager, PluginDefinition _pluginDefinition) : base( _pluginsManager, _eventsManager, _windowsManager, _projectsManager, _pluginDefinition)
        {
            pluginInfo = new Info();
        }

        public override int deinit()
        {
            return 0;
        }

        public override int init()
        {
            log.info($"{pluginInfo.Name}: Init plugin");

            envvarsEnabled = pluginDefinition.config.Get("envVar", false);
            log.debug($"{pluginInfo.Name}: Environment variables = {envvarsEnabled.ToString()}");

            builtinEnabled = pluginDefinition.config.Get("builtinVars", false);
            log.debug($"{pluginInfo.Name}: built-in variables = {builtinEnabled.ToString()}");

            specialFoldersEnabled = pluginDefinition.config.Get("specialFoldersVars", false);
            log.debug($"{pluginInfo.Name}: Windows special folders variables = {specialFoldersEnabled.ToString()}");

            userEnabled = pluginDefinition.config.Get("userVars", true);
            log.debug($"{pluginInfo.Name}: User defined variables = {userEnabled.ToString()}");

            log.info($"[{pluginInfo.Name}] Loading variables");
            populateVariables();

            log.debug($"{pluginInfo.Name}: Apply User Variables to onChange event of Text class");
            eventsManager.addEventHandler("Text.onChange", (object sender, EventArgs args) =>
            {
                OnChangeEventArgs arg = (OnChangeEventArgs)args;
                arg.newValue = digest(arg.newValue);
                return true;
            }, true);

            
            
            return 0;
        }

        /// <summary>
        /// Get a string and find all occurence of dictionary keys and substitude with its values 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string digest(string source)
        {
            foreach (var v in dictionary)
            {
                source = v.Value(source);
            }

            return source;
        }

        private void populateVariables()
        {
            
            // environment special folders
            if (specialFoldersEnabled)
            {
                foreach (System.Environment.SpecialFolder i in Enum.GetValues(typeof(System.Environment.SpecialFolder)))
                {
                    string name = $"%%{Enum.GetName(typeof(System.Environment.SpecialFolder), i)}";
                    if (dictionary.ContainsKey(name))
                    {
                        dictionary[name] = (string source) =>
                        {
                            return source.Replace(name, Environment.GetFolderPath(i));
                        };
                    }
                    else
                    {
                        dictionary.Add(name, (string source) =>
                        {
                            return source.Replace(name, Environment.GetFolderPath(i));
                        });
                    }
                        
                }
            }

            // built-in variables
            if (builtinEnabled)
            {
                dictionary.Add("%%CommandLine", (string source) =>
                {
                    return source.Replace("%%CommandLine", Environment.CommandLine);
                });

                dictionary.Add("%%CurrentDirectory", (string source) =>
                {
                    return source.Replace("%%currentDirectory", Environment.CurrentDirectory);
                });

                dictionary.Add("%%CurrentManagedThreadId", (string source) =>
                {
                    return source.Replace("%%CurrentManagedThreadId", Environment.CurrentManagedThreadId.ToString());
                });

                dictionary.Add("%%Is64BitOperatingSystem", (string source) =>
                {
                    return source.Replace("%%Is64BitOperatingSystem", Environment.Is64BitOperatingSystem.ToString());
                });

                dictionary.Add("%%Is64BitProcess", (string source) =>
                {
                    return source.Replace("%%Is64BitProcess", Environment.Is64BitProcess.ToString());
                });

                dictionary.Add("%%IsPrivilegedProcess", (string source) =>
                {
                    return source.Replace("%%IsPrivilegedProcess", Environment.IsPrivilegedProcess.ToString());
                });

                dictionary.Add("%%machineName", (string source) =>
                {
                    return source.Replace("%%MachineName", Environment.MachineName);
                });

                dictionary.Add("%%NewLine", (string source) =>
                {
                    return source.Replace("%%NewLine", Environment.NewLine);
                });

                dictionary.Add("%%ProcessId", (string source) =>
                {
                    return source.Replace("%%ProcessId", Environment.ProcessId.ToString());
                });

                dictionary.Add("%%ProcessorCount", (string source) =>
                {
                    return source.Replace("%%ProcessorCount", Environment.ProcessId.ToString());
                });

                dictionary.Add("%%ProcessPath", (string source) =>
                {
                    return source.Replace("%%ProcessPath", (Environment.ProcessPath == null) ? "" : Environment.ProcessPath);
                });

                dictionary.Add("%%SystemDirectory", (string source) =>
                {
                    return source.Replace("%%SystemDirectory", Environment.SystemDirectory);
                });
                
                dictionary.Add("%%UserDomainName", (string source) =>
                {
                    return source.Replace("%%UserDomainName", Environment.UserDomainName);
                });

                dictionary.Add("%%UserInteractive", (string source) =>
                {
                    return source.Replace("%%UserInteractive", Environment.UserInteractive.ToString());
                });

                dictionary.Add("%%UserName", (string source) =>
                {
                    return source.Replace("%%UserName", Environment.UserName);
                });

                dictionary.Add("%%WorkingSet", (string source) =>
                {
                    return source.Replace("%%WorkingSet", Environment.WorkingSet.ToString());
                });
            }

            // environment variables
            if (envvarsEnabled)
            {
            }

            // user variables
            if (userEnabled)
            {
                JObject userVars = pluginDefinition.config.Get("dictionary", new JObject());
                foreach (JPair v in userVars.GetProperties())
                {
                    if (dictionary.ContainsKey($"%%{v.Key}"))
                    {
                        dictionary[$"%%{v.Key}"] = (string source) =>
                        {
                            return source.Replace($"%%{v.Key}", v.Value);
                        };
                    }
                    else
                    {
                        dictionary.Add($"%%{v.Key}", (string source) =>
                        {
                            return source.Replace($"%%{v.Key}", v.Value);
                        });
                    }
                }
            }

            // log variables
            string bivStr = "";
            foreach (var v in dictionary)
            {
                bivStr += $"\"{v.Key}\" : \"{v.Value(v.Key)}\"\n";
            }
            log.debug($"{pluginInfo.Name}: List of variables\n{bivStr}");

        }      
    }

   
}
