
using Microsoft.Web.WebView2.Core;
using Pacioli.Config.Persistence;
using Pacioli.Language.Resources;
using Pacioli.WindowsApp.Util;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Pacioli.WindowsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var pref = UserPreferences.LoadPreferences();

            if (pref.PerformUpdateCheck)
            {
                /*
                 * The check must never stop the start of the program. A server that sleeps,
                 * a network without a way out, or a proxy that asks for a name: each of
                 * them gives an exception here, and the user would see no window at all.
                 * Thus the check keeps quiet if it fails.
                 */
                try
                {
                    var check = UpdateCheck.CreateAsync().Result;
                    // Safe to block here: no SynchronizationContext is installed before Application.Run,
                    // so GetAwaiter().GetResult() cannot deadlock.

                    if (check.NewVersionAvailable())
                    {
                        var result = MessageBox.Show(string.Format(Resources.msgUpdateAsk, VersionInformation.GetVersion(), check.LatestVersion), Resources.msgUpdateAnnounce, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            check.PerformUpdate().Wait();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(args));
        }
    }
}