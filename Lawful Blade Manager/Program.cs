using System.Windows.Forms;
using LawfulBladeManager.Networking;
using LawfulBladeManager.Project;

namespace LawfulBladeManager
{
    public static class Program
    {
        // Properties
        public static ProgramContext? Context;
        public static ProjectManager? ProjectManager;
        public static DownloadManager? DownloadManager;

        // Entry Point
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            DownloadManager = new();
            ProjectManager  = new();
            Context         = new();

            Application.Run(Context);
        }
    }
}