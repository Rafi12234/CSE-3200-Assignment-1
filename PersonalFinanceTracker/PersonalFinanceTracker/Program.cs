// Program.cs
// Entry point for the Personal Finance Tracker application.

using System;
using System.Windows.Forms;

namespace PersonalFinanceTracker
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable modern visual styles for controls (required for .NET Framework WinForms)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Launch the main form
            Application.Run(new Form1());
        }
    }
}
