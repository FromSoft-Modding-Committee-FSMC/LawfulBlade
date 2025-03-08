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

                // Technically only one of these is required, but I haven't figured out what field dictates that yet
                File.Copy(Path.Combine(PrjParam, "0.lvt"), Path.Combine(PubParam, "0.LVT"), true);
                File.Copy(Path.Combine(PrjParam, "1.lvt"), Path.Combine(PubParam, "1.LVT"), true);
                File.Copy(Path.Combine(PrjParam, "2.lvt"), Path.Combine(PubParam, "2.LVT"), true);
                File.Copy(Path.Combine(PrjParam, "3.lvt"), Path.Combine(PubParam, "3.LVT"), true);
                File.Copy(Path.Combine(PrjParam, "4.lvt"), Path.Combine(PubParam, "4.LVT"), true);

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
                UpdateStatus?.Invoke("Processing Enemy Definitions...");

                string EdtEnemy = Path.Combine(args.InstancePath, "DATA", "ENEMY");
                string PubEnemy = Path.Combine(args.PublishPath,  "DATA", "ENEMY");

                if (Directory.Exists(EdtEnemy))
                {
                    if (!Directory.Exists(PubEnemy))
                    {
                        Directory.CreateDirectory(PubEnemy);
                        Directory.CreateDirectory(Path.Combine(PubEnemy, "MODEL"));
                    }       

                    // We need to load the ENEMY.PR2 so we can parse what models we need...
                    EnemyPR2File EnemyPR2 = new (Path.Combine(PubParam, "ENEMY.PR2"));

                    foreach (EnemyPR2Item item in EnemyPR2.Items)
                    {
                        // Copy Model
                        File.Copy(
                            Path.Combine(EdtEnemy, "MODEL", item.modelName),
                            Path.Combine(PubEnemy, "MODEL", item.modelName),
                            true);
                        
                        // Copy Control Points (if they exist, they should...)
                        if (File.Exists(Path.Combine(EdtEnemy, "MODEL", Path.ChangeExtension(item.modelName, "cp").ToUpperInvariant())))
                        {
                            File.Copy(
                                Path.Combine(EdtEnemy, "MODEL", Path.ChangeExtension(item.modelName, "cp")),
                                Path.Combine(PubEnemy, "MODEL", Path.ChangeExtension(item.modelName, "cp").ToUpperInvariant()),
                                true);
                        }

                        // Copy external texture if it exists...
                        if (item.hasExternalTexture)
                        {
                            if (!Directory.Exists(Path.Combine(PubEnemy, "TEXTURE")))
                                Directory.CreateDirectory(Path.Combine(PubEnemy, "TEXTURE"));

                            File.Copy(
                                Path.Combine(EdtEnemy, "TEXTURE", item.externalTextureName),
                                Path.Combine(PubEnemy, "TEXTURE", item.externalTextureName.ToUpperInvariant()),
                                true);
                        }
                    }

                    // We also need to copy the default KAGE and KAGE2 models (used for shadows?..)
                    File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE.MDL"),  Path.Combine(PubEnemy, "MODEL", "KAGE.MDL"), true);
                    File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE2.MDL"), Path.Combine(PubEnemy, "MODEL", "KAGE2.MDL"), true);
                    File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE2.MDO"), Path.Combine(PubEnemy, "MODEL", "KAGE2.MDO"), true);
                    File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE3.MDL"), Path.Combine(PubEnemy, "MODEL", "KAGE3.MDL"), true);
                    File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE3.MDO"), Path.Combine(PubEnemy, "MODEL", "KAGE3.MDO"), true);
                }


                // DATA\\ITEM
                UpdateStatus?.Invoke("Processing Item Definitions...");

                string EdtItem = Path.Combine(args.InstancePath, "DATA", "ITEM");
                string PubItem = Path.Combine(args.PublishPath, "DATA", "ITEM");

                if (Directory.Exists(EdtItem))
                {
                    if (!Directory.Exists(PubItem))
                    {
                        Directory.CreateDirectory(PubItem);
                        Directory.CreateDirectory(Path.Combine(PubItem, "MODEL"));
                    }

                    // Load Item PR2 for wrangling
                    ItemPR2File ItemPR2 = new(Path.Combine(PubParam, "ITEM.PR2"));

                    foreach (ItemPR2Item item in ItemPR2.Items)
                    {
                        // Copy Model
                        File.Copy(
                            Path.Combine(EdtItem, "MODEL", item.modelFile),
                            Path.Combine(PubItem, "MODEL", item.modelFile),
                            true);

                        // Copy Model #2... These are used for swing animations, so swords mostly
                        string swingModel = $"{Path.GetFileNameWithoutExtension(item.modelFile)}_0{Path.GetExtension(item.modelFile)}";
                        if (File.Exists(Path.Combine(EdtItem, "MODEL", swingModel)))
                        {
                            // Copy Model
                            File.Copy(
                                Path.Combine(EdtItem, "MODEL", swingModel),
                                Path.Combine(PubItem, "MODEL", swingModel),
                                true);
                        }

                        // Copy Textures (using filenames inside MDO)
                        string[] textureFiles = Path.GetExtension(item.modelFile).ToUpperInvariant() switch
                        {
                            ".MDO" => MDOFile.GetTexturesFromMDO(Path.Combine(EdtItem, "MODEL", item.modelFile)),
                            _      => Array.Empty<string>(),
                        };

                        foreach (string textureFile in textureFiles)
                        {
                            File.Copy(
                                Path.Combine(EdtItem, "MODEL", textureFile),
                                Path.Combine(PubItem, "MODEL", textureFile),
                                true);
                        }
                    }

                    // SoM requires a few more files here too
                    File.Copy(Path.Combine(EdtItem, "MODEL", "gold.mdo"), Path.Combine(PubItem, "MODEL", "GOLD.MDO"), true);
                    File.Copy(Path.Combine(EdtItem, "MODEL", "gold.txr"), Path.Combine(PubItem, "MODEL", "GOLD.TXR"), true);
                    File.Copy(Path.Combine(EdtItem, "MODEL", "ude01.txr"), Path.Combine(PubItem, "MODEL", "UDE01.TXR"), true);                
                }


                // DATA\\MAP
                string PrjMap = Path.Combine(args.ProjectPath, "DATA", "MAP");
                string EdtMap = Path.Combine(args.InstancePath, "DATA", "MAP");
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

                    if (Directory.Exists(EdtMap))
                    {
                        // Copy all the textures from the editor... We won't filter these because fuck it.
                        CopyDir(new DirectoryInfo(Path.Combine(EdtMap, "TEXTURE")), Path.Combine(PubMap, "TEXTURE"));

                        if (!Directory.Exists(Path.Combine(PubMap, "MODEL")))
                            Directory.CreateDirectory(Path.Combine(PubMap, "MODEL"));

                        // And copy all the sky models...
                        foreach(FileInfo mdoFile in new DirectoryInfo(Path.Combine(EdtMap, "MODEL")).GetFiles("*.mdo"))
                        {
                            // Copy the actual MDO file
                            File.Copy(mdoFile.FullName, Path.Combine(PubMap, "MODEL", mdoFile.Name), true);

                            // Get MDO textures
                            string[] textureFiles = MDOFile.GetTexturesFromMDO(mdoFile.FullName);

                            foreach (string textureFile in textureFiles)
                            {
                                File.Copy(
                                    Path.Combine(EdtMap, "MODEL", textureFile),
                                    Path.Combine(PubMap, "MODEL", textureFile),
                                    true);
                            }
                        }
                    }
                        
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

                if (Directory.Exists(EdtMy))
                    CopyDir(new DirectoryInfo(EdtMy), PubMy);


                // DATA\\NPC
                UpdateStatus?.Invoke("Processing NPC Definitions...");

                string EdtNPC = Path.Combine(args.InstancePath, "DATA", "NPC");
                string PubNPC = Path.Combine(args.PublishPath, "DATA", "NPC");

                if (Directory.Exists(EdtNPC))
                {
                    if (!Directory.Exists(PubNPC))
                    {
                        Directory.CreateDirectory(PubNPC);
                        Directory.CreateDirectory(Path.Combine(PubNPC, "MODEL"));
                    }

                    // We need to load the ENEMY.PR2 so we can parse what models we need...
                    NpcPR2File NpcPR2 = new(Path.Combine(PubParam, "NPC.PR2"));

                    foreach (NpcPR2Item item in NpcPR2.Items)
                    {
                        // Copy Model
                        File.Copy(
                            Path.Combine(EdtNPC, "MODEL", item.modelName),
                            Path.Combine(PubNPC, "MODEL", item.modelName),
                            true);

                        // Copy Control Points
                        if (File.Exists(Path.Combine(EdtNPC, "MODEL", Path.ChangeExtension(item.modelName, "cp").ToUpperInvariant())))
                        {
                            File.Copy(
                                Path.Combine(EdtNPC, "MODEL", Path.ChangeExtension(item.modelName, "cp")),
                                Path.Combine(PubNPC, "MODEL", Path.ChangeExtension(item.modelName, "cp").ToUpperInvariant()),
                                true);
                        }

                        // Copy external texture
                        if (item.hasExternalTexture)
                        {
                            if (!Directory.Exists(Path.Combine(PubNPC, "TEXTURE")))
                                Directory.CreateDirectory(Path.Combine(PubNPC, "TEXTURE"));

                            File.Copy(
                                Path.Combine(EdtNPC, "TEXTURE", item.externalTextureName),
                                Path.Combine(PubNPC, "TEXTURE", item.externalTextureName.ToUpperInvariant()),
                                true);
                        }
                    }
                }


                // DATA\\OBJ
                UpdateStatus?.Invoke("Processing Object Definitions...");

                string EdtObject = Path.Combine(args.InstancePath, "DATA", "OBJ");
                string PubObject = Path.Combine(args.PublishPath, "DATA", "OBJ");

                if (Directory.Exists(EdtObject))
                {
                    if (!Directory.Exists(PubObject))
                    {
                        Directory.CreateDirectory(PubObject);
                        Directory.CreateDirectory(Path.Combine(PubObject, "MODEL"));
                    }

                    // We need to load the ENEMY.PR2 so we can parse what models we need...
                    ObjectPR2File ObjectPR2 = new(Path.Combine(PubParam, "OBJ.PR2"));

                    foreach (ObjectPR2Item item in ObjectPR2.Items)
                    {
                        // Copy Model
                        File.Copy(
                            Path.Combine(EdtObject, "MODEL", item.modelFile),
                            Path.Combine(PubObject, "MODEL", item.modelFile),
                            true);

                        // Copy Textures - Objects don't support external textures!!! :D
                        string[] textureFiles = Path.GetExtension(item.modelFile).ToUpperInvariant() switch
                        {
                            ".MDO" => MDOFile.GetTexturesFromMDO(Path.Combine(EdtObject, "MODEL", item.modelFile)),
                            _ => Array.Empty<string>(),
                        };

                        foreach (string textureFile in textureFiles)
                        {
                            File.Copy(
                                Path.Combine(EdtObject, "MODEL", textureFile),
                                Path.Combine(PubObject, "MODEL", textureFile),
                                true);
                        }
                    }
                }


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
                string PrjMusic = Path.Combine(args.ProjectPath, "DATA", "BGM");
                string PubMusic = Path.Combine(args.PublishPath, "DATA", "SOUND", "BGM");

                if (Directory.Exists(PrjMusic))
                    CopyDir(new DirectoryInfo(PrjMusic), PubMusic);

                //
                // Stage 3: These last files are weirdly handled
                //
                UpdateStatus?.Invoke("Finalizing...");

                // PROJECT.DAT
                // This has a really shit anti-tamper which is just the size of "npc.pr2" and "enemy.pr2" added together.
                int pdatTamperVal = 0;
                    pdatTamperVal += File.ReadAllBytes(Path.Combine(PubParam, "ENEMY.PR2")).Length;
                    pdatTamperVal += File.ReadAllBytes(Path.Combine(PubParam, "NPC.PR2")).Length;

                File.WriteAllBytes(Path.Combine(args.PublishPath, "PROJECT.DAT"), BitConverter.GetBytes(pdatTamperVal));

                // INI
                File.Copy(Path.Combine(args.ProjectPath, "SOM_DB.INI"), Path.Combine(args.PublishPath, $"{args.ProjectName}.INI"), true);

                // EXE
                File.Copy(Path.Combine(args.InstancePath, "TOOL", "som_rt.exe"), Path.Combine(args.PublishPath, $"{args.ProjectName}.EXE"), true);
                PatchExecutableResources(args);
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

        public void PatchExecutableResources(GeneratorStartArgs args)
        {
            string rceditPath = Path.Combine(ProgramPath, "Tools", "rcedit.exe");

            Process resourceEditProcess = Process.Start(rceditPath,
                [
                    $"{Path.Combine(args.PublishPath, $"{args.ProjectName}.EXE")}",                             // Executable
                    "--set-icon", $"{Path.Combine(args.ProjectPath, "project.ico")}",                           // Set Icon
                    "--set-version-string", "FileDescription",  $"Sword of Moonlight Game",
                    "--set-version-string", "LegalCopyright",   $"©{DateTime.Now.Year} {args.AuthorName}",      // Set Copyright
                    "--set-version-string", "OriginalFilename", $"{args.ProjectName}.exe",                      // Set Original Filename
                    "--set-version-string", "ProductName", $"{args.ProjectName}",                               // Set Product Name
                    "--set-version-string", "FileVersion", $"1.00",
                    "--set-version-string", "ProductVersion", $"1.00",
                    "--set-version-string", "CompanyName", $"{args.AuthorName}"
                ]);

            resourceEditProcess.WaitForExit();
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
