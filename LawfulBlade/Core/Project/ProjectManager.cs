using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core
{
    public static class ProjectManager
    {
        /// <summary>
        /// Event invoked when a project is added or removed
        /// </summary>
        public static event Action ProjectsChanged;

        /// <summary>
        /// List of all avaliable projects
        /// </summary>
        public static List<Project> Projects { get; private set; }

        /// <summary>
        /// The number of projects
        /// </summary>
        public static int Count => Projects.Count;

        /// <summary>
        /// Initializes the project manager, loading all projects in to memory
        /// </summary>
        public static void Initialize()
        {
            // Create the instance list
            Projects = [];

            if (!Directory.Exists(App.ProjectPath))
                Directory.CreateDirectory(App.ProjectPath);

            // Go through each subdirectory of the projects folder...
            foreach (DirectoryInfo projectDir in new DirectoryInfo(App.ProjectPath).GetDirectories())
            {
                // If the folder does not contain 'project.json' we will not process it.
                if (!File.Exists(Path.Combine(projectDir.FullName, "project.json")))
                    continue;

                // The project.json file does exist, we can load this as an project...
                Project loadedProject = Project.Load(projectDir.FullName);

                // If the project returned null, do not add it to the list...
                if (loadedProject != null)
                    Projects.Add(loadedProject);
            }

            // Sort projects by the LastAccess time
            Projects.Sort(Comparer<Project>.Create((a, b) => (int)(b.LastAccessDateTime.Ticks - a.LastAccessDateTime.Ticks)));
        }

        /// <summary>
        /// Call to shutdown the project manager
        /// </summary>
        public static void Shutdown()
        {
            // Attempt to save all our instances...
            foreach (Project project in Projects)
                project.Save();

            // Clear the instances list
            Projects.Clear();
            Projects = null;
        }

        /// <summary>
        /// Adds an project to the list of managed projects...
        /// </summary>
        public static void AddProject(Project instance)
        {
            // Add instance
            Projects.Add(instance);

            // Raise the instance changed event...
            ProjectsChanged?.Invoke();
        }

        /// <summary>
        /// Removes a project from the list of managed projects...
        /// </summary>
        public static void RemoveProject(Project instance)
        {
            // Remove instance
            Projects.Remove(instance);

            // Raise the instance changed event...
            ProjectsChanged?.Invoke();
        }

        /// <summary>
        /// Gets a project from the list of managed projects by the UUID
        /// </summary>
        public static Project GetProjectByUUID(string UUID)
        {
            foreach (Project project in Projects)
            {
                if (project.UUID == UUID)
                    return project;
            }

            return null;
        }
    }
}
