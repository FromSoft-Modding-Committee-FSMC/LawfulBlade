using System.Windows.Forms;

using LawfulBladeManager.Project;

namespace LawfulBladeManager
{
    public static class Program
    {
        // Private Data
        static readonly ProgramContext context = new();

        // Properties
        public static ProgramContext Context => context;

        // Entry Point
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(context);
        }
    }
}