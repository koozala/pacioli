
using Microsoft.Web.WebView2.Core;
using Pacioli.Config.Persistence;
using Pacioli.WindowsApp.NET8.Util;
using System.Reflection;
using System.Text;

namespace Pacioli.WindowsApp.NET8
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
                var check = new UpdateCheck();
                // Safe to block here: no SynchronizationContext is installed before Application.Run,
                // so GetAwaiter().GetResult() cannot deadlock.
                check.ExecuteAsync(true).GetAwaiter().GetResult();
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(args));
        }
    }
}