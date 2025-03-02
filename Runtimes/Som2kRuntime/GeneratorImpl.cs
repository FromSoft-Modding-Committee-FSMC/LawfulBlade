using LawfulBladeSDK.Generator;
using Som2kRuntime.Formats;
using System.Diagnostics;

namespace Som2kRuntime
{
    public class GeneratorImpl : IRuntimeGenerator
    {
        static GeneratorProperty transcodeMovies = new() { Name = "Transcode Movies?", Type = typeof(bool), Value = true };

        /// <summary>
        /// Returns our array of custom properties
        /// </summary>
        public GeneratorProperty[] Properties { get; set; } =
            [
                transcodeMovies
            ];

        /// <summary>
        /// Som2k runtime **only supports Som2k games**!!!
        /// </summary>
        public string[] SupportedCores { get; } = [ "24d8608f-8dc0-4b21-8333-05f097fd1823" ];

        /// <summary>
        /// Som2k runtime name
        /// </summary>
        public string Name { get; } = "SoM2k Runtime";

        /// <summary>
        /// Event raised when a file is given by lawful blade to complete generation.
        /// </summary>
        public event Action<string> UpdateStatus;

        // Generation Data
        string ProgramPath;
        string TemporaryPath;

        /// <summary>
        /// Called when the generator is first loaded.
        /// </summary>
        public void OnLoad(GeneratorLoadArgs args)
        {
            ProgramPath   = args.ProgramPath;
            TemporaryPath = args.TemporaryPath;
        }

        public void StartGenerator(GeneratorStartArgs args)
        {
            // Stage 1: Create Output Directories...
            if (!Directory.Exists(args.PublishPath))
                Directory.CreateDirectory(args.PublishPath);
            
            // Stage 2: Huge Copy...
            try
            {
                // Parameters Copy...
                UpdateStatus?.Invoke("Processing Parameters...");

                string PubParam = Path.Combine(args.PublishPath, "PARAM");
                string PrjParam = Path.Combine(args.ProjectPath, "PARAM");

                if (!Directory.Exists(PubParam))
                    Directory.CreateDirectory(PubParam);
                
                File.Copy(Path.Combine(PrjParam, "ENEMY.PR2"), Path.Combine(PubParam, "ENEMY.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "ENEMY.PRM"), Path.Combine(PubParam, "ENEMY.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "ITEM.PR2"), Path.Combine(PubParam, "ITEM.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "ITEM.PRM"), Path.Combine(PubParam, "ITEM.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "MAGIC.PR2"), Path.Combine(PubParam, "MAGIC.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "MAGIC.PRM"), Path.Combine(PubParam, "MAGIC.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "NPC.PR2"), Path.Combine(PubParam, "NPC.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "NPC.PRM"), Path.Combine(PubParam, "NPC.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "OBJ.PR2"), Path.Combine(PubParam, "OBJ.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "OBJ.PRM"), Path.Combine(PubParam, "OBJ.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "SHOP.DAT"), Path.Combine(PubParam, "SHOP.DAT"), true);
                File.Copy(Path.Combine(PrjParam, "SYS.DAT"), Path.Combine(PubParam, "SYS.DAT"), true);

                // SYS.DAT stuff...
                UpdateStatus?.Invoke("Processing System Settings...");

                SYSDatFile SysDat = new(Path.Combine(PubParam, "SYS.DAT"));

                if (SysDat.OpenSequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "TITLE.DAT"), Path.Combine(PubParam, "TITLE.DAT"), true);

                if (SysDat.IntroSequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "OPENNING.DAT"), Path.Combine(PubParam, "OPENNING.DAT"), true);

                if (SysDat.End1SequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "ENDING1.DAT"), Path.Combine(PubParam, "ENDING1.DAT"), true);

                if (SysDat.End2SequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "ENDING2.DAT"), Path.Combine(PubParam, "ENDING2.DAT"), true);

                if (SysDat.End3SequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "ENDING3.DAT"), Path.Combine(PubParam, "ENDING3.DAT"), true);

                if (SysDat.StaffSequenceMode == 2)
                    File.Copy(Path.Combine(PrjParam, "STAFF.DAT"), Path.Combine(PubParam, "STAFF.DAT"), true);

                // DATA\\ENEMY

                // DATA\\ITEM

                // DATA\\MAP
                string PrjMap = Path.Combine(args.ProjectPath, "DATA", "MAP");
                string PubMap = Path.Combine(args.PublishPath, "DATA", "MAP");

                if (Directory.Exists(PrjMap))
                {
                    if (!Directory.Exists(PubMap))
                        Directory.CreateDirectory(PubMap);

                    // Compile MAP -> MPX
                    foreach (FileInfo file in (new DirectoryInfo(PrjMap)).GetFiles("*.map"))
                        CompileMap(file.FullName, args.ProjectPath, args.InstancePath, Path.Combine(PubMap, $"{Path.GetFileNameWithoutExtension(file.FullName)}.mpx"));

                    // Copy all the EVT files...
                    foreach (FileInfo file in (new DirectoryInfo(PrjMap)).GetFiles("*.evt"))
                        File.Copy(file.FullName, Path.Combine(PubMap, $"{Path.GetFileName(file.FullName)}"), true);
                }

                // DATA\\MENU
                UpdateStatus?.Invoke("Processing Menu...");
                string EdtMenu = Path.Combine(args.InstancePath, "DATA", "MENU");
                string PubMenu = Path.Combine(args.PublishPath, "DATA", "MENU");

                if (Directory.Exists(EdtMenu))
                    CopyDir(new DirectoryInfo(EdtMenu), PubMenu);


                // DATA\\MOVIE
                string PrjMovie = Path.Combine(args.ProjectPath, "DATA", "MOVIE");
                string PubMovie = Path.Combine(args.PublishPath, "DATA", "MOVIE");

                if (Directory.Exists(PrjMovie))
                {
                    if (!Directory.Exists(PubMovie))
                        Directory.CreateDirectory(PubMovie);

                    if (((bool)transcodeMovies.Value) == true)
                    {
                        // Transcode movies into H264+16le PCM
                        UpdateStatus?.Invoke("Transcoding Video Files...");

                        foreach (FileInfo videoFile in (new DirectoryInfo(PrjMovie)).GetFiles())
                            TranscodeVideo(videoFile.FullName, Path.Combine(PubMovie, $"{Path.GetFileNameWithoutExtension(videoFile.FullName)}.avi"));
                    }
                    else
                    {
                        // Copy movies as they are
                        UpdateStatus?.Invoke("Copying Video Files...");

                        foreach (FileInfo videoFile in (new DirectoryInfo(PrjMovie)).GetFiles())
                            File.Copy(videoFile.FullName, Path.Combine(PubMovie, $"{Path.GetFileName(videoFile.FullName)}"), true);
                    }
                }


                // DATA\\MY
                UpdateStatus?.Invoke("Processing Arms...");
                string EdtMy = Path.Combine(args.InstancePath, "DATA", "MY", "MODEL");
                string PubMy = Path.Combine(args.PublishPath, "DATA", "MY", "MODEL");

                if (Directory.Exists(PubMy))
                    CopyDir(new DirectoryInfo(EdtMy), PubMy);
                

                // DATA\\NPC

                // DATA\\OBJ

                // DATA\\PICTURE
                UpdateStatus?.Invoke("Processing Pictures...");
                string PrjPicture = Path.Combine(args.ProjectPath, "DATA", "PICTURE");
                string PubPicture = Path.Combine(args.PublishPath, "DATA", "PICTURE");

                if (Directory.Exists(PrjPicture))
                    CopyDir(new DirectoryInfo(PrjPicture), PubPicture);


                // DATA\\SFX
                UpdateStatus?.Invoke("Processing Special Effects...");
                string EdtFX = Path.Combine(args.InstancePath, "DATA", "SFX");
                string PubFX = Path.Combine(args.PublishPath, "DATA", "SFX");

                if (Directory.Exists(EdtFX))
                    CopyDir(new DirectoryInfo(EdtFX), PubFX);


                // DATA\\SOUND
                UpdateStatus?.Invoke("Processing Sounds...");
                string EdtSound = Path.Combine(args.InstancePath, "DATA","SOUND", "SE");
                string PubSound = Path.Combine(args.PublishPath, "DATA", "SOUND", "SE");

                if (!Directory.Exists(PubSound))
                    Directory.CreateDirectory(PubSound);

                foreach (FileInfo file in (new DirectoryInfo(EdtSound)).GetFiles("*.snd"))
                    File.Copy(file.FullName, Path.Combine(PubSound, $"{Path.GetFileName(file.FullName)}"), true);

                UpdateStatus?.Invoke("Processing Music...");
                string PrjMusic = Path.Combine(args.InstancePath, "DATA", "SOUND", "BGM");
                string PubMusic = Path.Combine(args.PublishPath,  "DATA", "SOUND", "BGM");

                if (Directory.Exists(PrjMusic))
                    CopyDir(new DirectoryInfo(PrjMusic), PubMusic);


                // INI, EXE
                UpdateStatus?.Invoke("Finalizing...");
                File.Copy(Path.Combine(args.ProjectPath, "SOM_DB.INI"), Path.Combine(args.PublishPath, "GAME.INI"), true);
                File.Copy(Path.Combine(args.InstancePath, "TOOL", "som_rt.exe"), Path.Combine(args.PublishPath, "GAME.EXE"), true);

                //
                // done !!!!
                //
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish project: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void TranscodeVideo(string inputFile, string outputFile)
        {
            string ffmpegPath = Path.Combine(ProgramPath, "Tools", "ffmpeg.exe");

            Process transcodeProcess = Process.Start(ffmpegPath, 
                [
                    "-y", "-loglevel", "warning", "-stats", // Force overwrite, warnings + errors + encoding status
                    "-i", $"{inputFile}",                   // Input
                    "-c:v", "libx264",                      // Use H264 codec for video
                    "-c:a", "pcm_s16le",                    // Use PCM 16-Bit signed for audio
                    $"{outputFile}"                         // Output
                ]);

            transcodeProcess.WaitForExit();
        }

        public void CompileMap(string inputFile, string projectDir, string instanceDir, string outputFile)
        {
            string mapcompPath = Path.Combine(instanceDir, "tool", "MapComp.exe");

            Process compileProcess = Process.Start(mapcompPath,
                [
                    inputFile,
                    outputFile,
                    instanceDir,
                    projectDir
                ]);

            compileProcess.WaitForExit();
        }

        public static void CopyDir(DirectoryInfo root, string destination)
        {
            // Make sure the root directory exists...
            if (!root.Exists)
                throw new DirectoryNotFoundException();

            // Create the target directory...
            Directory.CreateDirectory(destination);

            // Copy all files inside the directory...
            foreach (FileInfo file in root.GetFiles())
                file.CopyTo(Path.Combine(destination, file.Name), true);

            // Copy all subdirectories (recursive
            foreach (DirectoryInfo dir in root.GetDirectories())
                CopyDir(dir, Path.Combine(destination, dir.Name));
        }
    }
}
