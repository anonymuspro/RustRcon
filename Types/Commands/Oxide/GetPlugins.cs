#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RustRcon.Pooling;
using RustRcon.Types.Commands.Base;
using RustRcon.Types.Oxide;
using RustRcon.Types.Response.Server;

#endregion

namespace RustRcon.Types.Commands.Oxide
{
    public class GetPlugins : BaseCommand
    {
        public PoolableList<Plugin>? Result { get; private set; }

        /// <summary>
        ///     Returns a list of installed plugins (if there is an oxide mod)
        /// </summary>
        public static GetPlugins Create()
        {
            var command = CreatePackage<GetPlugins>();
            command.Content = "o.plugins";

            return command;
        }

        public override void Complete(ServerResponse response)
        {
            base.Complete(response);

            if (string.IsNullOrEmpty(response.Content) || response.Content.StartsWith("Listing") == false)
                return;

            try
            {
                Result = RustRconPool.Get<PoolableList<Plugin>>();
                var lines = response.Content.AsSpan().Trim().Split("\n");
                var linesEnumerator = lines.GetEnumerator();
                linesEnumerator.MoveNext();

                while (linesEnumerator.MoveNext())
                {
                    var lineSpan = linesEnumerator.Current.AsSpan().Trim();

                    int titleStart = lineSpan.IndexOf('"') + 1;
                    int titleEnd = lineSpan.Slice(titleStart).IndexOf('"') + titleStart;
                    var title = lineSpan.Slice(titleStart, titleEnd - titleStart).ToString();

                    int versionStart = lineSpan.Slice(titleEnd).IndexOf('(') + titleEnd + 1;
                    int versionEnd = lineSpan.Slice(versionStart).IndexOf(')') + versionStart;
                    var version = lineSpan.Slice(versionStart, versionEnd - versionStart).ToString();

                    int authorStart = lineSpan.Slice(versionEnd).IndexOf("by ") + versionEnd + 3;
                    int authorEnd = lineSpan.Slice(authorStart).IndexOf('(') + authorStart - 1;
                    var author = lineSpan.Slice(authorStart, authorEnd - authorStart).Trim().ToString();

                    int timeStart = lineSpan.Slice(authorEnd).IndexOf('(') + authorEnd + 1;
                    int timeEnd = lineSpan.Slice(timeStart).IndexOf('s') + timeStart + 1;
                    var time = lineSpan.Slice(timeStart, timeEnd - timeStart).ToString();

                    int memoryStart = lineSpan.Slice(timeEnd).IndexOf('/') + timeEnd + 2;
                    int memoryEnd = lineSpan.Slice(memoryStart).IndexOf(' ') + memoryStart + 1;
                    var memory = lineSpan.Slice(memoryStart, memoryEnd - memoryStart).Trim().ToString();

                    int filenameStart = lineSpan.Slice(memoryEnd).IndexOf("- ") + memoryEnd + 2;
                    var filename = lineSpan.Slice(filenameStart).ToString().Trim();

                    Result.Add(new Plugin(filename, title, version, author, time, memory));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void EnterPool()
        {
            base.EnterPool();
            Result?.Dispose();
        }
    }
}