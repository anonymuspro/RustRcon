using RustRcon.Types.Commands.Base;
using RustRcon.Types.Oxide;
using RustRcon.Types.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RustRcon.Pooling;

namespace RustRcon.Types.Commands.Oxide
{
    public class GetPlugins : BaseCommand
    {
        private Action<List<Plugin>> _callback;

        /// <summary>
        /// Returns a list of installed plugins (if there is an oxide mod)
        /// </summary>
        /// <param name="callback"></param>
        public static GetPlugins Create(Action<List<Plugin>> callback = null)
        {
            var command = CreatePackage<GetPlugins>();
            command._callback = callback;
            command.Content = "o.plugins";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);
            List<Plugin> pluginList = RustRconPool.GetList<Plugin>();

            if (string.IsNullOrEmpty(response.Content) || response.Content.StartsWith("Listing") == false)
            {
                _callback?.Invoke(pluginList);
                RustRconPool.FreeList(pluginList);
                return;
            }

            try
            {
                List<string> plugins = response.Content.Split('\n').ToList();

                foreach (var plugin in plugins.Skip(1))
                {
                    string name;
                    bool loaded;
                    string version = "";
                    string author = "";

                    Match m1 = Regex.Match(plugin,
                        "[0-9]+\\s\"(.*)\"\\s\\(([0-9]+.[0-9]+.[0-9]+)\\)\\sby\\s(.*?)\\s\\((.*?)\\)\\s-\\s(.*)");

                    if (m1.Success == false)
                    {
                        m1 = Regex.Match(plugin, "[0-9]+\\s(.*)\\s-\\sUnloaded");

                        if (m1.Success == false)
                            continue;

                        name = m1.Groups[1].Value;
                        loaded = false;
                    }
                    else
                    {
                        name = m1.Groups[1].Value;
                        loaded = true;
                        version = m1.Groups[2].Value;
                        author = m1.Groups[3].Value;
                    }


                    pluginList.Add(new Plugin(name, loaded, version, author));
                }

                _callback?.Invoke(pluginList);
                RustRconPool.FreeList(pluginList);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void EnterPool()
        {
            base.EnterPool();

            _callback = null;
        }
    }
}