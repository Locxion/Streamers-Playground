using System;
using System.IO;

namespace Assets.Scripts
{
    public class Constants
    {
        private static readonly string AppdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // AppData folder
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); // Desktop folder
        public static readonly string ProgramfilesfolderPath = Path.Combine(AppdataPath, "Playground");     // Path for program config folder
        private static readonly string LogfolderPath = Path.Combine(ProgramfilesfolderPath, "logs"); // Path for old Logfiles

        public static readonly string LogPath = Path.Combine(LogfolderPath, "playground.log");

        private const string SettingsFile = "settings.txt";
        public static string SettingsPath = Path.Combine(ProgramfilesfolderPath, SettingsFile);
    }
}