using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LawfulBladeManager.Project
{
    public class ProjectManager
    {
        // Private Data
        List<Project>? projects;

        // Properties
        public List<Project>? Projects => projects;

        // Constructors
        public ProjectManager()
        {
            if(!LoadProjectInfo() || projects == null)
                projects = new List<Project>();
        }

        /// <summary>
        /// Create a new SOM Project
        /// </summary>
        /// <param name="path">Target path for the project</param>
        /// <param name="name">Name of the project</param>
        /// <param name="description">Description of the project</param>
        public void CreateProject(string path, string name, string description)
        {
            path = Path.Combine(path, name);

            // Make sure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Write PROJECT.DAT
            using(StreamWriter sw = new(File.Open(Path.Combine(path, $"{name}.som"), FileMode.CreateNew)))
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
            CreateDefaultObj(Path.Combine(path, @"PARAM"));
            CreateDefaultItem(Path.Combine(path, @"PARAM"));
            CreateDefaultMagic(Path.Combine(path, @"PARAM"));

            // Copy default parameters

            // Save default icon
            Bitmap? bm = (Bitmap?)Properties.Resources.ResourceManager.GetObject("defaultProjectIcon");
            bm?.Save(Path.Combine(path, "icon.png"), ImageFormat.Png);

            // Create the project configuration
            projects?.Add(new Project
            {
                Name = name,
                Description = description,
                Author = Environment.UserName,
                InstanceUUID = "",
                LastEditData = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"),
                StoragePath = path,
                IsManaged   = true
            });
        }

        /// <summary>
        /// Saves information on all projects
        /// </summary>
        public void SaveProjectInfo()
        {
            string projectsAsJson = JsonSerializer.Serialize(projects, JsonSerializerOptions.Default);
            File.WriteAllText(Path.Combine(ProgramContext.AppDataPath, @"projects.json"), projectsAsJson);
        }

        /// <summary>
        /// Loads information on all projects
        /// </summary>
        /// <returns>True on success, false otherwise.</returns>
        public bool LoadProjectInfo()
        {
            if (!File.Exists(Path.Combine(ProgramContext.AppDataPath, @"projects.json")))
                return false;

            projects = JsonSerializer.Deserialize<List<Project>>(File.ReadAllText(Path.Combine(ProgramContext.AppDataPath, @"projects.json")));
            return true;
        }

        /// <summary>
        /// Creates a SOM "sequence" .DAT file. 
        /// These are used to store information about movie playback and image display for standard cutscenes.
        /// </summary>
        /// <param name="movieFilePath">The path to the movie file, including file name.</param>
        static void CreateDefaultSequence(string movieFilePath)
        {
            using (StreamWriter sw = new(File.Open(movieFilePath, FileMode.CreateNew)))
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
        /// <param name="shopFilePath">The path to the shop file, including file name.</param>
        static void CreateDefaultShop(string shopFilePath)
        {
            using(FileStream s = File.Open(shopFilePath, FileMode.CreateNew))
                s.SetLength(0x2FF00);
        }

        /// <summary>
        /// Creates a SOM "object" .PRM, and .PR2 file.
        /// These are used for storing object configuration
        /// </summary>
        /// <param name="objFilePath">The path to where the file will go, without filename.</param>
        static void CreateDefaultObj(string objFilePath)
        {
            // OBJ.PRM
            using (BinaryWriter bw = new(File.Open(Path.Combine(objFilePath, "OBJ.PRM"), FileMode.CreateNew)))
            {
                for(int i = 0; i < 1024; ++i)
                {
                    bw.Seek(0x20, SeekOrigin.Current);
                    bw.Write(1f);
                    bw.Write((short)-1);
                    bw.Write((ulong)0x0000000000000000);
                    bw.Write((ulong)0x0000000000000000);
                    bw.Write((short)0x0000);
                }
            }

            // OBJ.PR2
            using (FileStream s = File.Open(Path.Combine(objFilePath, "OBJ.PR2"), FileMode.CreateNew))
                s.SetLength(0x4);
        }

        /// <summary>
        /// Creates a SOM "item" .PRM, and .PR2 file.
        /// These are used for storing item configuration
        /// </summary>
        /// <param name="itemFilePath">The path to where the file will go, without filename.</param>
        static void CreateDefaultItem(string itemFilePath)
        {
            // ITEM.PRM
            using (BinaryWriter bw = new(File.Open(Path.Combine(itemFilePath, "ITEM.PRM"), FileMode.CreateNew)))
            {
                for (int i = 0; i < 250; ++i)
                {
                    bw.Write((short)-1);
                    bw.Seek(0x134, SeekOrigin.Current);
                    bw.Write((sbyte)-1);
                    bw.Write((byte)0);      
                    bw.Write((ulong)0x0000000000000000);
                    bw.Write((ulong)0x0000000000000000);
                    bw.Write((ulong)0x0000000000000000);
                }
            }

            // ITEM.PR2
            using (FileStream s = File.Open(Path.Combine(itemFilePath, "ITEM.PR2"), FileMode.CreateNew))
                s.SetLength(0x4);
        }

        /// <summary>
        /// Creates a SOM "magic" .PRM and .PR2 file.
        /// These are used for storing magic configuration
        /// </summary>
        /// <param name="magicFilePath">The path to where the file will go, without filename.</param>
        static void CreateDefaultMagic(string magicFilePath)
        {
            // MAGIC.PRM
            using (BinaryWriter bw = new(File.Open(Path.Combine(magicFilePath, "MAGIC.PRM"), FileMode.CreateNew)))
            {
                for (int i = 0; i < 250; ++i)
                {
                    bw.Write((short)-1);
                    bw.Seek(0x13A, SeekOrigin.Current);
                    bw.Write((uint)0);
                }
            }

            // MAGIC.PR2
            using (FileStream s = File.Open(Path.Combine(magicFilePath, "MAGIC.PR2"), FileMode.CreateNew))
                s.SetLength(0x4);
        }
    }
}
