
using Pacioli.WindowsApp.NET8.Util;
using System.Reflection;

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

            var check = new UpdateCheck();
            check.Execute(true);

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}