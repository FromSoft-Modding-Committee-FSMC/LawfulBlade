using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.Format.SoM;
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
            CreateDirectoryLogged(args, args.PublishPath);
            
            try
            {
                // Stage 2: Huge Copy..
                CompileParameterData(args);
                CompileObjectData(args);
                CompileItemData(args);
                CompileEnemyData(args);
                CompileNPCData(args);
                CompileSpecialEffectsData(args);
                CompileSoundData(args);          
                CompileArmData(args);
                CompileMapData(args);
                CompileMenuData(args);
                CompilePictureData(args);
                CompileMovieData(args); 

                //
                // Stage 3: These last files are weirdly handled
                //
                UpdateStatus?.Invoke("Finalizing...");

                // PROJECT.DAT
                // This has a really shit anti-tamper which is just the size of "npc.pr2" and "enemy.pr2" added together.
                int pdatTamperVal = 0;
                    pdatTamperVal += File.ReadAllBytes(Path.Combine(args.PublishPath, "PARAM", "ENEMY.PR2")).Length;
                    pdatTamperVal += File.ReadAllBytes(Path.Combine(args.PublishPath, "PARAM", "NPC.PR2")).Length;

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

        public void CompileParameterData(GeneratorStartArgs args)
        {
            // Parameters Copy...
            UpdateStatus?.Invoke("Processing Parameters...");

            string PrjParam = Path.Combine(args.ProjectPath, "PARAM");
            string PubParam = Path.Combine(args.PublishPath, "PARAM");

            CreateDirectoryLogged(args, PubParam);
            CopyFileLogged(args, Path.Combine(PrjParam, "ENEMY.PR2"), Path.Combine(PubParam, "ENEMY.PR2"));
            CopyFileLogged(args, Path.Combine(PrjParam, "ENEMY.PRM"), Path.Combine(PubParam, "ENEMY.PRM"));
            CopyFileLogged(args, Path.Combine(PrjParam, "ITEM.PR2"), Path.Combine(PubParam, "ITEM.PR2"));
            CopyFileLogged(args, Path.Combine(PrjParam, "ITEM.PRM"), Path.Combine(PubParam, "ITEM.PRM"));
            CopyFileLogged(args, Path.Combine(PrjParam, "MAGIC.PR2"), Path.Combine(PubParam, "MAGIC.PR2"));
            CopyFileLogged(args, Path.Combine(PrjParam, "MAGIC.PRM"), Path.Combine(PubParam, "MAGIC.PRM"));
            CopyFileLogged(args, Path.Combine(PrjParam, "NPC.PR2"), Path.Combine(PubParam, "NPC.PR2"));
            CopyFileLogged(args, Path.Combine(PrjParam, "NPC.PRM"), Path.Combine(PubParam, "NPC.PRM"));
            CopyFileLogged(args, Path.Combine(PrjParam, "OBJ.PR2"), Path.Combine(PubParam, "OBJ.PR2"));
            CopyFileLogged(args, Path.Combine(PrjParam, "OBJ.PRM"), Path.Combine(PubParam, "OBJ.PRM"));
            CopyFileLogged(args, Path.Combine(PrjParam, "SHOP.DAT"), Path.Combine(PubParam, "SHOP.DAT"));
            CopyFileLogged(args, Path.Combine(PrjParam, "SYS.DAT"), Path.Combine(PubParam, "SYS.DAT"));

            // Technically only one of these is required, but I haven't figured out what field dictates that yet
            CopyFileLogged(args, Path.Combine(PrjParam, "0.lvt"), Path.Combine(PubParam, "0.LVT"));
            CopyFileLogged(args, Path.Combine(PrjParam, "1.lvt"), Path.Combine(PubParam, "1.LVT"));
            CopyFileLogged(args, Path.Combine(PrjParam, "2.lvt"), Path.Combine(PubParam, "2.LVT"));
            CopyFileLogged(args, Path.Combine(PrjParam, "3.lvt"), Path.Combine(PubParam, "3.LVT"));
            CopyFileLogged(args, Path.Combine(PrjParam, "4.lvt"), Path.Combine(PubParam, "4.LVT"));

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
        }

        public void CompileObjectData(GeneratorStartArgs args)
        {
            // Update Lawful Blade that we are processing object definitions
            UpdateStatus?.Invoke("Processing Object Definitions...");
            Console.WriteLine("Processing object definitions...".Colourise(0xF0F0F0));

            // First we need to get the editor path to object data, and ensure that it exists...
            string edtObject      = Path.Combine(args.InstancePath, "DATA", "OBJ");
            string edtObjectModel = Path.Combine(edtObject, "MODEL");

            if (!Directory.Exists(edtObject))
                throw new Exception("Editor object data is missing.");

            // We should now read in the object profiles
            if (!FileFormatObjectPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "OBJ.PR2"), out FileFormatObjectPR2 objectPR2))
                throw new Exception("Couldn't read OBJ.PR2 data.");

            // Now we need to set up and create the publish directories...
            string pubObject      = Path.Combine(args.PublishPath, "DATA", "OBJ");
            string pubObjectModel = Path.Combine(pubObject, "MODEL");

            if (!Directory.Exists(pubObject))
            {
                CreateDirectoryLogged(args, pubObject);
                CreateDirectoryLogged(args, pubObjectModel);
            }
            
            // Use each profile to prepare object data
            foreach (ObjectPRF objectPRF in objectPR2.Profiles)
            {
                // Copy Base Model
                CopyFileLogged(args, Path.Combine(edtObjectModel, objectPRF.modelFile), Path.Combine(pubObjectModel, objectPRF.modelFile));

                if (!File.Exists(Path.Combine(edtObjectModel, objectPRF.modelFile)))
                    continue;

                // Copy additive models (_n)
                int additiveModelID = 0;
                string additiveModel;

                do
                {
                    // Get the name of an additive model
                    additiveModel = $"{Path.GetFileNameWithoutExtension(objectPRF.modelFile)}_{additiveModelID}{Path.GetExtension(objectPRF.modelFile)}";

                    if (!File.Exists(additiveModel))
                        break;

                    CopyFileLogged(args, Path.Combine(edtObjectModel, additiveModel), Path.Combine(pubObjectModel, additiveModel));

                    additiveModelID++;
                } while (true);

                // Copy CP files, where they exist
                string modelCPFile = Path.ChangeExtension(objectPRF.modelFile, "cp");

                if (File.Exists(Path.Combine(edtObjectModel, modelCPFile)))
                    CopyFileLogged(args, Path.Combine(edtObjectModel, modelCPFile), Path.Combine(pubObjectModel, modelCPFile));

                // Copy Textures
                string[] textureFiles = Array.Empty<string>();

                switch (Path.GetExtension(objectPRF.modelFile).ToUpperInvariant())
                {
                    case ".MDO":
                        if (FileFormatMDO.LoadFromFile(Path.Combine(edtObjectModel, objectPRF.modelFile), out FileFormatMDO mdo))
                            textureFiles = mdo.Textures;
                        break;
                }

                foreach (string textureFile in textureFiles)
                    CopyFileLogged(args, Path.Combine(edtObjectModel, textureFile), Path.Combine(pubObjectModel, textureFile));
            }
        }

        public void CompileItemData(GeneratorStartArgs args)
        {
            // Update Lawful Blade that we are processing item definitions
            UpdateStatus?.Invoke("Processing Item Definitions...");
            Console.WriteLine("Processing item definitions...".Colourise(0xF0F0F0));

            // First we need to get the editor path to object data, and ensure that it exists...
            string edtItem = Path.Combine(args.InstancePath, "DATA", "ITEM");
            string edtItemModel = Path.Combine(edtItem, "MODEL");

            if (!Directory.Exists(edtItem))
                throw new Exception("Editor item data is missing.");

            // We should now read in the item profiles
            if (!FileFormatItemPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PR2"), out FileFormatItemPR2 itemPR2))
                throw new Exception("Couldn't read ITEM.PR2 data.");

            string pubItem      = Path.Combine(args.PublishPath, "DATA", "ITEM");
            string pubItemModel = Path.Combine(pubItem, "MODEL");

            if (!Directory.Exists(pubItem))
            {
                CreateDirectoryLogged(args, pubItem);
                CreateDirectoryLogged(args, pubItemModel);
            }

            foreach (ItemPRF itemPRF in itemPR2.Profiles)
            {
                // Copy Base Model
                CopyFileLogged(args, Path.Combine(edtItemModel, itemPRF.ModelFile), Path.Combine(pubItemModel, itemPRF.ModelFile));

                if (!File.Exists(Path.Combine(edtItemModel, itemPRF.ModelFile)))
                    continue;

                // Copy additive models (_n)
                int additiveModelID = 0;
                string additiveModel;

                do
                {
                    // Get the name of an additive model
                    additiveModel = $"{Path.GetFileNameWithoutExtension(itemPRF.ModelFile)}_{additiveModelID}{Path.GetExtension(itemPRF.ModelFile)}";

                    if (!File.Exists(additiveModel))
                        break;

                    CopyFileLogged(args, Path.Combine(edtItemModel, additiveModel), Path.Combine(pubItemModel, additiveModel));

                    additiveModelID++;
                } while (true);

                // Copy CP files, where they exist
                string modelCPFile = Path.ChangeExtension(itemPRF.ModelFile, "cp");

                if (File.Exists(Path.Combine(edtItemModel, modelCPFile)))
                    CopyFileLogged(args, Path.Combine(edtItemModel, modelCPFile), Path.Combine(pubItemModel, modelCPFile));

                // Copy Textures
                string[] textureFiles = Array.Empty<string>();

                switch (Path.GetExtension(itemPRF.ModelFile).ToUpperInvariant())
                {
                    case ".MDO":
                        if (FileFormatMDO.LoadFromFile(Path.Combine(edtItemModel, itemPRF.ModelFile), out FileFormatMDO mdo))
                            textureFiles = mdo.Textures;
                        break;
                }

                foreach (string textureFile in textureFiles)
                    CopyFileLogged(args, Path.Combine(edtItemModel, textureFile), Path.Combine(pubItemModel, textureFile));
            }

            // SoM requires a few more files here too
            CopyFileLogged(args, Path.Combine(edtItemModel, "gold.mdo"),  Path.Combine(pubItemModel, "GOLD.MDO"));
            CopyFileLogged(args, Path.Combine(edtItemModel, "gold.txr"),  Path.Combine(pubItemModel, "GOLD.TXR"));
            CopyFileLogged(args, Path.Combine(edtItemModel, "ude01.txr"), Path.Combine(pubItemModel, "UDE01.TXR"));
        }

        public void CompileEnemyData(GeneratorStartArgs args)
        {
            UpdateStatus?.Invoke("Processing Enemy Definitions...");

            string EdtEnemy = Path.Combine(args.InstancePath, "DATA", "ENEMY");
            string PubEnemy = Path.Combine(args.PublishPath, "DATA", "ENEMY");

            if (Directory.Exists(EdtEnemy))
            {
                if (!Directory.Exists(PubEnemy))
                {
                    Directory.CreateDirectory(PubEnemy);
                    Directory.CreateDirectory(Path.Combine(PubEnemy, "MODEL"));
                }

                // We need to load the ENEMY.PR2 so we can parse what models we need...
                EnemyPR2File EnemyPR2 = new(Path.Combine(args.PublishPath, "PARAM", "ENEMY.PR2"));

                foreach (EnemyPR2Item item in EnemyPR2.Items)
                {
                    // Copy Model
                    File.Copy(
                        Path.Combine(EdtEnemy, "MODEL", item.modelName),
                        Path.Combine(PubEnemy, "MODEL", item.modelName).ToUpperInvariant(),
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
                File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE.MDL"), Path.Combine(PubEnemy, "MODEL", "KAGE.MDL"), true);
                File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE2.MDL"), Path.Combine(PubEnemy, "MODEL", "KAGE2.MDL"), true);
                File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE2.MDO"), Path.Combine(PubEnemy, "MODEL", "KAGE2.MDO"), true);
                File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE3.MDL"), Path.Combine(PubEnemy, "MODEL", "KAGE3.MDL"), true);
                File.Copy(Path.Combine(EdtEnemy, "MODEL", "KAGE3.MDO"), Path.Combine(PubEnemy, "MODEL", "KAGE3.MDO"), true);
            }
        }

        public void CompileNPCData(GeneratorStartArgs args)
        {
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
                NpcPR2File NpcPR2 = new(Path.Combine(args.PublishPath, "PARAM", "NPC.PR2"));

                foreach (NpcPR2Item item in NpcPR2.Items)
                {
                    // Copy Model
                    File.Copy(
                        Path.Combine(EdtNPC, "MODEL", item.modelName),
                        Path.Combine(PubNPC, "MODEL", item.modelName).ToUpperInvariant(),
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
        }

        public void CompileMapData(GeneratorStartArgs args)
        {
            UpdateStatus?.Invoke("Processing Map Data...");
            Console.WriteLine("Processing map data...".Colourise(0xF0F0F0));

            // Validate the project map directorie
            string prjMap = Path.Combine(args.ProjectPath,  "DATA", "MAP");

            if (!Directory.Exists(prjMap))
                throw new Exception("Project map data is missing.");

            DirectoryInfo projectMapInfo = new DirectoryInfo(prjMap);

            // Validate the editor map directories
            string edtMap = Path.Combine(args.InstancePath, "DATA", "MAP");

            if (!Directory.Exists(edtMap))
                throw new Exception("Editor map data is missing.");

            // Create the publish directory
            string pubMap        = Path.Combine(args.PublishPath, "DATA", "MAP");
            string pubMapTexture = Path.Combine(pubMap, "TEXTURE");
            string pubMapModel   = Path.Combine(pubMap, "MODEL");

            if (!Directory.Exists(pubMap))
            {
                Directory.CreateDirectory(pubMap);
                Directory.CreateDirectory(pubMapTexture);
            }
                

            // Compile MAP -> MPX
            foreach (FileInfo mapFile in projectMapInfo.EnumerateFiles("*.map"))
                CompileMap(mapFile.FullName, args.ProjectPath, args.InstancePath, Path.Combine(pubMap, $"{Path.GetFileNameWithoutExtension(mapFile.FullName)}.mpx"));

            // Copy EVT
            foreach (FileInfo evtFile in projectMapInfo.EnumerateFiles("*.evt"))
                CopyFileLogged(args, evtFile.FullName, Path.Combine(pubMap, $"{Path.GetFileName(evtFile.FullName)}"));

            // Copy all the textures from the editor... We won't filter these because fuck it.
            CopyDir(new DirectoryInfo(Path.Combine(edtMap, "TEXTURE")), Path.Combine(pubMap, "TEXTURE"));

            // And copy all the sky models...
            foreach (FileInfo mdoFile in new DirectoryInfo(Path.Combine(edtMap, "MODEL")).GetFiles("*.mdo"))
            {
                // Copy the actual MDO file
                File.Copy(mdoFile.FullName, Path.Combine(pubMap, "MODEL", mdoFile.Name), true);

                // Get MDO textures
                string[] textureFiles = Array.Empty<string>();

                switch (Path.GetExtension(mdoFile.FullName).ToUpperInvariant())
                {
                    case ".MDO":
                        if (FileFormatMDO.LoadFromFile(mdoFile.FullName, out FileFormatMDO mdo))
                            textureFiles = mdo.Textures;
                        break;
                }

                foreach (string textureFile in textureFiles)
                {
                    File.Copy(
                        Path.Combine(edtMap, "MODEL", textureFile),
                        Path.Combine(pubMap, "MODEL", textureFile),
                        true);
                }
            }
        }

        public void CompileMenuData(GeneratorStartArgs args)
        {
            UpdateStatus?.Invoke("Processing Menu...");
            string EdtMenu = Path.Combine(args.InstancePath, "DATA", "MENU");
            string PubMenu = Path.Combine(args.PublishPath, "DATA", "MENU");

            if (Directory.Exists(EdtMenu))
                CopyDir(new DirectoryInfo(EdtMenu), PubMenu);
        }

        public void CompileArmData(GeneratorStartArgs args)
        {
            UpdateStatus?.Invoke("Processing Arms...");
            string EdtMy = Path.Combine(args.InstancePath, "DATA", "MY", "MODEL");
            string PubMy = Path.Combine(args.PublishPath, "DATA", "MY", "MODEL");

            if (Directory.Exists(EdtMy))
                CopyDir(new DirectoryInfo(EdtMy), PubMy);
        }

        public void CompileMovieData(GeneratorStartArgs args)
        {
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
        }

        public void CompilePictureData(GeneratorStartArgs args)
        {
            // DATA\\PICTURE
            UpdateStatus?.Invoke("Processing Pictures...");
            string PrjPicture = Path.Combine(args.ProjectPath, "DATA", "PICTURE");
            string PubPicture = Path.Combine(args.PublishPath, "DATA", "PICTURE");

            if (Directory.Exists(PrjPicture))
                CopyDir(new DirectoryInfo(PrjPicture), PubPicture);
        }

        public void CompileSpecialEffectsData(GeneratorStartArgs args)
        {
            UpdateStatus?.Invoke("Processing Special Effects...");
            string EdtFX = Path.Combine(args.InstancePath, "DATA", "SFX");
            string PubFX = Path.Combine(args.PublishPath, "DATA", "SFX");

            if (Directory.Exists(EdtFX))
                CopyDir(new DirectoryInfo(EdtFX), PubFX);
        }

        public void CompileSoundData(GeneratorStartArgs args)
        {
            // Sound Effects
            UpdateStatus?.Invoke("Processing Sounds...");
            string EdtSound = Path.Combine(args.InstancePath, "DATA", "SOUND", "SE");
            string PubSound = Path.Combine(args.PublishPath, "DATA", "SOUND", "SE");

            if (!Directory.Exists(PubSound))
                Directory.CreateDirectory(PubSound);

            foreach (FileInfo file in (new DirectoryInfo(EdtSound)).GetFiles("*.snd"))
                File.Copy(file.FullName, Path.Combine(PubSound, $"{Path.GetFileName(file.FullName)}"), true);

            // Music
            UpdateStatus?.Invoke("Processing Music...");
            string PrjMusic = Path.Combine(args.ProjectPath, "DATA", "BGM");
            string PubMusic = Path.Combine(args.PublishPath, "DATA", "SOUND", "BGM");

            if (Directory.Exists(PrjMusic))
                CopyDir(new DirectoryInfo(PrjMusic), PubMusic);
        }

        public void TranscodeVideo(string inputFile, string outputFile)
        {
            string ffmpegPath = Path.Combine(ProgramPath, "Tools", "ffmpeg.exe");

            Process transcodeProcess = Process.Start(ffmpegPath, 
                [
                    "-y", "-loglevel", "warning", "-stats", // Force overwrite, warnings + errors + encoding status
                    "-i", $"{inputFile}",                   // Input
                    "-c:v", "wmv2",                         // Use Microsoft MPEG4 (wmv) codec for video
                    "-b:v", "2048k",                        // High Bit Rate because FUck, man
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

            string pechksumPath = Path.Combine(ProgramPath, "Tools", "pechksum.exe");

            Process pechksumProcess = Process.Start(pechksumPath,
                ["/nologo",
                $"{Path.Combine(args.PublishPath, $"{args.ProjectName}.EXE")}"]
                );

            pechksumProcess.WaitForExit();
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

        public void CopyDir(DirectoryInfo root, string destination)
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

        public void CopyFileLogged(GeneratorStartArgs args, string source, string destination)
        {
            Console.WriteLine($"\tCopy File {{ Source = '{source.Replace(args.InstancePath, "")}', Destination = '{destination.Replace(args.PublishPath, "")}' }}".Colourise(0x8080F0));

            if (File.Exists(source))
                File.Copy(source, destination.ToUpperInvariant(), true);
            else
                Console.WriteLine($"\tSource file missing!!! ^^");
        }

        public void CreateDirectoryLogged(GeneratorStartArgs args, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Create Directroy {{ Target = '{directory.Replace(args.PublishPath, "")}'}}");
                Directory.CreateDirectory(directory.ToUpperInvariant());
            }
        }
    }
}
