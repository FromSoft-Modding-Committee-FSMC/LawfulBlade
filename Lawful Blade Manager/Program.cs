using LawfulBladeManager.Project;

namespace LawfulBladeManager
{
    public static class Program
    {
        // Private Data
        static string pathAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FSMC", "LawfulBlade");

        // Public Data
        public static ProjectManager projectManager = new();

        // Properties
        public static string AppDataPath => pathAppData;

        // Implementation

        [STAThread]
        static void Main()
        {
            InitializePaths();

            projectManager.CreateProject(@"C:\Users\lXDayDreamXl\Desktop", "Test Project", "");
            projectManager.SaveProjectInfo();

            ApplicationConfiguration.Initialize();
            Application.Run(new wfMainForm());
        }

        static void InitializePaths()
        {
            if(!Directory.Exists(pathAppData))
                Directory.CreateDirectory(pathAppData);
        }
    }
}