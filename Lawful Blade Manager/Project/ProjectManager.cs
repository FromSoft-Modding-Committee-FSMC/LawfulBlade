using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LawfulBladeManager.Project
{
    public class ProjectManager
    {
        // Private Data
        List<Project> projects;

        // Properties
        public List<Project> Projects => projects;

        // Constructors
        public ProjectManager()
        {
            if(!LoadProjectInfo())
                projects = new List<Project>();
        }

        
        // Static Implementation
        public void CreateProject(string path, string name, string description)
        {
            path = Path.Combine(path, name);

            // Make sure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Write PROJECT.DAT
            using(StreamWriter sw = new StreamWriter(File.Open(Path.Combine(path, $"{name}.som"), FileMode.CreateNew)))
            {
                sw.WriteLine(name);
                sw.WriteLine('0');
            }

            // Create data directories
            Directory.CreateDirectory(Path.Combine(path, @"DATA"));
            Directory.CreateDirectory(Path.Combine(path, @"DATA", @"BGM"));
            Directory.CreateDirectory(Path.Combine(path, @"DATA", @"MAP"));
            Directory.CreateDirectory(Path.Combine(path, @"DATA", @"MOVIE"));
            Directory.CreateDirectory(Path.Combine(path, @"DATA", @"PICTURE"));

            // Create simple parameters
            Directory.CreateDirectory(Path.Combine(path, @"PARAM"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "ENDING1.DAT"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "ENDING2.DAT"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "ENDING3.DAT"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "OPENNING.DAT"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "STAFF.DAT"));
            CreateDefaultSequence(Path.Combine(path, @"PARAM", "TITLE.DAT"));
            CreateDefaultShop(Path.Combine(path, @"PARAM", "SHOP.DAT"));

            // Copy default parameters


            // Create the project configuration
            projects.Add(new Project
            {
                Name = name,
                Description = description,
                Author = Environment.UserName,
                InstanceUUID = "",
                LastEditData = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"),
                StoragePath = path
            });
        }

        /// <summary>
        /// Saves information on all projects
        /// </summary>
        public void SaveProjectInfo()
        {
            string projectsAsJson = JsonSerializer.Serialize(projects, JsonSerializerOptions.Default);
            File.WriteAllText(Path.Combine(Program.AppDataPath, "projects_ref.json"), projectsAsJson);
        }

        public bool LoadProjectInfo()
        {
            if (!File.Exists(Path.Combine(Program.AppDataPath, "projects_ref.json")))
                return false;

            string projectsAsJson = File.ReadAllText(Path.Combine(Program.AppDataPath, "projects_ref.json"));
            List<Project>? temp = JsonSerializer.Deserialize<List<Project>>(projectsAsJson);

            if (temp == null)
                return false;

            projects = temp;

            return true;
        }

        /// <summary>
        /// Creates a SOM "sequence" .DAT file. 
        /// These are used to store information about movie playback and image display for standard cutscenes.
        /// </summary>
        /// <param name="movieFilePath">The path to the movie file, including file name.</param>
        static void CreateDefaultSequence(string movieFilePath)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(movieFilePath, FileMode.CreateNew)))
            {
                sw.WriteLine(@"1");
                sw.WriteLine(@"NO_BMP");
                sw.WriteLine(@"0");
                sw.WriteLine(@"NO_BGM");
                sw.WriteLine();
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine(@" ");
                sw.WriteLine();
            }
        }

        /// <summary>
        /// Creates a SOM "shop" .DAT file.
        /// This file is used to store shop configuration.
        /// </summary>
        /// <param name="shopFilePath"></param>
        static void CreateDefaultShop(string shopFilePath)
        {
            using(FileStream s = File.Open(shopFilePath, FileMode.OpenOrCreate))
                s.SetLength(0x2FF00);
        }
    }
}
