using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.Format.SoM;
using LawfulBladeSDK.Generator;
using LawfulBladeSDK.IO;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.IO.Compression;

namespace SomExRuntime
{
    public class GeneratorImpl : IRuntimeGenerator
    {
        public enum SomExVersion
        {
            V1_2_1_14,
            V1_2_2_14,
            V1_2_3_10,
            V1_2_4_12,
            V1_2_5_14,
            V1_2_6_6
        }

        /// <summary>Property allows user to choose the SoM Ex version to use</summary>
        [GeneratorPropertyTooltip("The SomEx version compile the game with.\nThis can be used to change versions if a bug or nuisance feature exists in a particular version.")]
        static GeneratorProperty somExVersion          = new() { Name = "SomEx Version",    Type = typeof(SomExVersion), Value = SomExVersion.V1_2_3_10 };

        /// <summary>Property allows user to choose to transcode movies</summary>
        [GeneratorPropertyTooltip("Set if movies should be transcoded to a 'perfect' format for SoM/Windows.\nUses WMV (Windows MPEG4) and PCM-16le for maximum compatibility across OS versions.")]
        static GeneratorProperty transcodeMovies       = new() { Name = "Transcode Movies", Type = typeof(bool), Value = true };

        /// <summary>Property allows user to choose to apply fixes or not</summary>
        [GeneratorPropertyTooltip("Set if you want to apply common fixes to your Sword of Moonlight game.\nYou shouldn't disable this unless you are experimenting.")]
        static GeneratorProperty applyFixes            = new() { Name = "Apply Data Fixes", Type = typeof(bool), Value = true };

        /// <summary>Property allows user to choose to include the default runtime as well</summary>
        [GeneratorPropertyTooltip("Set if you want to include the vanilla runtime as well as the ex runtime for maximum compatibility.")]
        static GeneratorProperty includeVanillaRuntime = new() { Name = "Include Vanilla Runtime", Type = typeof(bool), Value = true };

        /// <summary>Property allows user to choose to enable verbose logging to debug compile issues</summary>
        [GeneratorPropertyTooltip("Set if you want to see every stage of project compilation (file copy, directory copy, file create - useful when debugging export problems.)\nYOU MUST ENABLE THE CONSOLE IN PREFERENCES TO SEE THIS INFORMATION!")]
        static GeneratorProperty verboseLogging        = new() { Name = "Verbose Logging", Type = typeof(bool), Value = true };

        /// <summary>array of custom properties accessible from Lawful Blade</summary>
        public GeneratorProperty[] Properties { get; set; } =
            [
                somExVersion,
                transcodeMovies,
                applyFixes,
                includeVanillaRuntime,
                verboseLogging
            ];

        /// <summary>runtime **only supports Som2k games**!!!</summary>
        public string[] SupportedCores { get; } = 
            [
                "24d8608f-8dc0-4b21-8333-05f097fd1823"
            ];

        /// <summary>runtime name</summary>
        public string Name { get; } = "SoM Ex Runtime";

        /// <summary>Event raised when a file is given by lawful blade to complete generation.</summary>
        public event Action<string> UpdateStatus;

        /// <summary>Cached path to Lawful Blade</summary>
        string ProgramPath;

        /// <summary>
        /// Called when the generator is first loaded.
        /// </summary>
        public void OnLoad(GeneratorLoadArgs args)
        {
            ProgramPath = args.ProgramPath;
        }

        /// <summary>
        /// Called to begin project generation
        /// </summary>
        public void StartGenerator(GeneratorStartArgs args)
        {
            // Create the publish directory tree ?
            FileSystem.CreateDirectory(args.PublishPath, 1);

            SendStatusUpdate("Beginning Runtime Generation...");
            Console.WriteLine($"SomEx Version    = {((SomExVersion)somExVersion.Value)}".Colourise(0xF0F080));
            Console.WriteLine($"Transcode Movies = {((bool)transcodeMovies.Value)}".Colourise(0xF0F080));
            Console.WriteLine($"Apply Data Fixes = {((bool)applyFixes.Value)}".Colourise(0xF0F080));
            Console.WriteLine($"Includes Vanilla = {((bool)includeVanillaRuntime.Value)}".Colourise(0xF0F080));
            Console.WriteLine($"Verbose Logging  = {((bool)verboseLogging.Value)}".Colourise(0xF0F080));

            // Is verbose logging enabled/disabled ? It changes how the FileSystem class works.
            FileSystem.EnableLogging = ((bool)verboseLogging.Value);

            // We want a try catch to capture any errors that might occure
            try
            {
                // Stage #1
                CompileParameters(args);
                CompileObjects(args);
                CompileItems(args);
                CompileEnemies(args);
                CompileNPCs(args);
                CompileEffects(args);
                CompileSounds(args);
                CompileArm(args);
                CompileMaps(args);
                CompileMenus(args);
                CompilePictures(args);
                CompileMovies(args);

                // Stage #2 (SomEx Runtime)
                CompileExRuntime(args);

                // Stage #3 (Som2k Runtime
                if (((bool)includeVanillaRuntime.Value))
                    CompileVanillaRuntime(args);

                // End
                Console.WriteLine("FINISHED!");
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to compile project:".Colourise(0xF08080));
                Console.WriteLine($"\t{ex.Message}".Colourise(0xF0F000));
            }
        }

        /// <summary>
        /// Compiles game parameters
        /// </summary>
        void CompileParameters(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling parameters...");

            // Get the project parameter directory
            string prjParam = Path.Combine(args.ProjectPath, "PARAM");
            if (!Directory.Exists(prjParam))
                throw new Exception("Project parameters do not exist.");

            // Here is where we need to open sys.dat for more information about the project... we won't right now.
            if (!FileFormatSysDAT.LoadFromFile(Path.Combine(prjParam, "SYS.DAT"), out FileFormatSysDAT sysDat))
                throw new Exception("sys.dat is either missing or corrupt.");

            // Create publish directory
            string pubParam = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "PARAM"), 1);

            // Copying the majority of all files
            FileSystem.CopyFile([prjParam, "OBJ.PR2"], [pubParam, "OBJ.PR2"], 2, 2);
            FileSystem.CopyFile([prjParam, "OBJ.PR2"], [pubParam, "OBJ.PRO"], 2, 2);    // WEIRD SoM Ex thing. Not sure which it uses...
            FileSystem.CopyFile([prjParam, "OBJ.PRM"], [pubParam, "OBJ.PRM"], 2, 2);
            FileSystem.CopyFile([prjParam, "ITEM.PR2"], [pubParam, "ITEM.PR2"], 2, 2);
            FileSystem.CopyFile([prjParam, "ITEM.PR2"], [pubParam, "ITEM.PRO"], 2, 2);
            FileSystem.CopyFile([prjParam, "ITEM.PRM"], [pubParam, "ITEM.PRM"], 2, 2);
            FileSystem.CopyFile([prjParam, "ENEMY.PR2"], [pubParam, "ENEMY.PR2"], 2, 2);
            FileSystem.CopyFile([prjParam, "ENEMY.PR2"], [pubParam, "ENEMY.PRO"], 2, 2);
            FileSystem.CopyFile([prjParam, "ENEMY.PRM"], [pubParam, "ENEMY.PRM"], 2, 2);
            FileSystem.CopyFile([prjParam, "NPC.PR2"], [pubParam, "NPC.PR2"], 2, 2);
            FileSystem.CopyFile([prjParam, "NPC.PR2"], [pubParam, "NPC.PRO"], 2, 2);
            FileSystem.CopyFile([prjParam, "NPC.PRM"], [pubParam, "NPC.PRM"], 2, 2);
            FileSystem.CopyFile([prjParam, "MAGIC.PR2"], [pubParam, "MAGIC.PR2"], 2, 2);
            FileSystem.CopyFile([prjParam, "MAGIC.PR2"], [pubParam, "MAGIC.PRO"], 2, 2);
            FileSystem.CopyFile([prjParam, "MAGIC.PRM"], [pubParam, "MAGIC.PRM"], 2, 2);
            FileSystem.CopyFile([prjParam, "SHOP.DAT"], [pubParam, "SHOP.DAT"], 2, 2);
            FileSystem.CopyFile([prjParam, "SYS.DAT"], [pubParam, "SYS.DAT"], 2, 2);

            // Copying required sequences
            if (sysDat.titleSequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "TITLE.DAT"], [pubParam, "TITLE.DAT"], 2, 2);

            if (sysDat.openingSequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "OPENNING.DAT"], [pubParam, "OPENNING.DAT"], 2, 2);

            if (sysDat.gameEnd1Sequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "ENDING1.DAT"], [pubParam, "ENDING1.DAT"], 2, 2);

            if (sysDat.gameEnd2Sequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "ENDING2.DAT"], [pubParam, "ENDING2.DAT"], 2, 2);

            if (sysDat.gameEnd3Sequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "ENDING3.DAT"], [pubParam, "ENDING3.DAT"], 2, 2);

            if (sysDat.staffSequence.sequenceMode == SysDatSequenceMode.Slideshow)
                FileSystem.CopyFile([prjParam, "STAFF.DAT"], [pubParam, "STAFF.DAT"], 2, 2);

            // Copying required level table
            FileSystem.CopyFile([prjParam, $"{sysDat.playerSettingA.levelTableID}.LVT"], [pubParam, $"{sysDat.playerSettingA.levelTableID}.LVT"], 2, 2);
        }

        /// <summary>
        /// Compiles object data
        /// </summary>
        void CompileObjects(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling objects...");

            // Get the editor object data directories
            string edtObject      = Path.Combine(args.InstancePath, "DATA", "OBJ");
            string edtObjectModel = Path.Combine(edtObject, "MODEL");

            if (!Directory.Exists(edtObject) || !Directory.Exists(edtObjectModel))
                throw new Exception("Editor object data is missing.");

            // Open up OBJ.PR2 to find out which models we need
            if (!FileFormatObjectPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "OBJ.PR2"), out FileFormatObjectPR2 pr2))
                throw new Exception("obj.pr2 is either missing or corrupt.");

            // Create publish directory(s)
            string pubObject      = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "OBJ"), 2);
            string pubObjectModel = FileSystem.CreateDirectory(Path.Combine(pubObject, "MODEL"));

            // Begin compiling objects
            foreach (ObjectPRF objPRF in pr2.Profiles)
            {
                // Base Model
                if (!FileSystem.CopyFile([edtObjectModel, objPRF.modelFile], [pubObjectModel, objPRF.modelFile], 4, 4))
                    continue;

                // Additive Models
                CopyAdditiveAssets(edtObjectModel, pubObjectModel, objPRF.modelFile, 4, 4);

                // Control Points
                string objectCP = Path.ChangeExtension(objPRF.modelFile, "cp");

                if (FileSystem.FileExists([edtObjectModel, objectCP]))
                    FileSystem.CopyFile([edtObjectModel, objectCP], [pubObjectModel, objectCP], 4, 4);

                // Textures
                foreach (string textureFile in FindTexturesForObject(edtObjectModel, objPRF))
                    FileSystem.CopyFile([edtObjectModel, textureFile], [pubObjectModel, textureFile], 4, 4);
            }
        }

        /// <summary>
        /// Finds textures for an object model
        /// </summary>
        string[] FindTexturesForObject(string modelBasePath, ObjectPRF prf)
        {
            string[] textureFiles = Array.Empty<string>();

            switch (Path.GetExtension(prf.modelFile).ToUpperInvariant())
            {
                case ".MDO":
                    if (FileFormatMDO.LoadFromFile(Path.Combine(modelBasePath, prf.modelFile), out FileFormatMDO mdo))
                        textureFiles = mdo.Textures;
                    break;
            }

            return textureFiles;
        }

        /// <summary>
        /// Compiles item data
        /// </summary>
        void CompileItems(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling items...");

            // Get the editor item data directories
            string edtItem      = Path.Combine(args.InstancePath, "DATA", "ITEM");
            string edtItemModel = Path.Combine(edtItem, "MODEL");

            if (!Directory.Exists(edtItem) || !Directory.Exists(edtItemModel))
                throw new Exception("Editor item data is missing.");

            // Open up ITEM.PR2 to find out which models we need
            if (!FileFormatItemPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PR2"), out FileFormatItemPR2 pr2))
                throw new Exception("item.pr2 is either missing or corrupt.");

            // Open up ITEM.PRM to apply de-duplication
            if (!FileFormatItemPRM.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PRM"), out FileFormatItemPRM prm))
                throw new Exception("item.prm is either missing or corrupt.");

            // DATA FIX: Item PRF deduplication
            if (((bool)applyFixes.Value) == true)
            {
                Console.WriteLine($"Deduplicated {prm.DeduplicateProfiles(ref pr2)} PRFs.".Colourise(0xF08020));

                if (pr2.SaveToBuffer(out byte[] pr2Buffer))
                {
                    FileSystem.CreateFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PR2"), pr2Buffer, 2);
                    FileSystem.CreateFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PRO"), pr2Buffer, 2);
                }

                if (prm.SaveToBuffer(out byte[] prmBuffer))
                    FileSystem.CreateFile(Path.Combine(args.PublishPath, "PARAM", "ITEM.PRM"), prmBuffer, 2);
            }

            // Create publish directory(s)
            string pubItem      = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "ITEM"), 2);
            string pubItemModel = FileSystem.CreateDirectory(Path.Combine(pubItem, "MODEL"));

            // Begin compiling items
            foreach (ItemPRF itemPRF in pr2.Profiles)
            {
                // Base Model
                if (!FileSystem.CopyFile([edtItemModel, itemPRF.ModelFile], [pubItemModel, itemPRF.ModelFile], 4, 4))
                    continue;

                // Additive Models
                CopyAdditiveAssets(edtItemModel, pubItemModel, itemPRF.ModelFile, 4, 4);

                // Control Points
                string itemCP = Path.ChangeExtension(itemPRF.ModelFile, "cp");

                if (FileSystem.FileExists([edtItemModel, itemCP]))
                    FileSystem.CopyFile([edtItemModel, itemCP], [pubItemModel, itemCP], 4, 4);

                // Textures
                foreach (string textureFile in FindTexturesForItem(edtItemModel, itemPRF))
                    FileSystem.CopyFile([edtItemModel, textureFile], [pubItemModel, textureFile], 4, 4);
            }

            // Essential default resources
            FileSystem.CopyFile([edtItemModel, "GOLD.MDO"], [pubItemModel, "GOLD.MDO"], 4, 4);
            FileSystem.CopyFile([edtItemModel, "GOLD.TXR"], [pubItemModel, "GOLD.TXR"], 4, 4);
            FileSystem.CopyFile([edtItemModel, "UDE01.TXR"], [pubItemModel, "UDE01.TXR"], 4, 4);
        }

        /// <summary>
        /// Finds textures for an item model
        /// </summary>
        string[] FindTexturesForItem(string modelBasePath, ItemPRF prf)
        {
            string[] textureFiles = Array.Empty<string>();

            switch (Path.GetExtension(prf.ModelFile).ToUpperInvariant())
            {
                case ".MDO":
                    if (FileFormatMDO.LoadFromFile(Path.Combine(modelBasePath, prf.ModelFile), out FileFormatMDO mdo))
                        textureFiles = mdo.Textures;
                    break;
            }

            return textureFiles;
        }

        /// <summary>
        /// Compiles enemy data
        /// </summary>
        void CompileEnemies(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling enemies...");

            // Get the editor enemy data directories
            string edtEnemy        = Path.Combine(args.InstancePath, "DATA", "ENEMY");
            string edtEnemyModel   = Path.Combine(edtEnemy, "MODEL");
            string edtEnemyTexture = Path.Combine(edtEnemy, "TEXTURE");

            if (!Directory.Exists(edtEnemy) || !Directory.Exists(edtEnemyModel))
                throw new Exception("Editor enemy data is missing.");

            // Open up ENEMY.PR2 to find out which enemies we need
            if (!FileFormatEnemyPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "ENEMY.PR2"), out FileFormatEnemyPR2 pr2))
                throw new Exception("enemy.pr2 is either missing or corrupt.");

            // Create publish directory(s)
            string pubEnemy        = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "ENEMY"), 2);
            string pubEnemyModel   = FileSystem.CreateDirectory(Path.Combine(pubEnemy, "MODEL"));
            string pubEnemyTexture = FileSystem.CreateDirectory(Path.Combine(pubEnemy, "TEXTURE"));

            foreach (EnemyPRF enemyPRF in pr2.Profiles)
            {
                // Base Model
                if (!FileSystem.CopyFile([edtEnemyModel, enemyPRF.ModelFile], [pubEnemyModel, enemyPRF.ModelFile], 4, 4))
                    continue;

                // Control Points
                string itemCP = Path.ChangeExtension(enemyPRF.ModelFile, "cp");

                if (FileSystem.FileExists([edtEnemyModel, itemCP]))
                    FileSystem.CopyFile([edtEnemyModel, itemCP], [pubEnemyModel, itemCP], 4, 4);

                // External Texture
                if (enemyPRF.useExternalTexture != 0)
                    FileSystem.CopyFile([edtEnemyTexture, enemyPRF.TextureFile], [pubEnemyTexture, enemyPRF.TextureFile], 4, 4);
            }

            // Essential default resources
            FileSystem.CopyFile([edtEnemyModel, "KAGE.MDL"], [pubEnemyModel, "KAGE.MDL"], 4, 4);
            FileSystem.CopyFile([edtEnemyModel, "KAGE2.MDL"], [pubEnemyModel, "KAGE2.MDL"], 4, 4);
            FileSystem.CopyFile([edtEnemyModel, "KAGE2.MDO"], [pubEnemyModel, "KAGE2.MDO"], 4, 4);
            FileSystem.CopyFile([edtEnemyModel, "KAGE3.MDL"], [pubEnemyModel, "KAGE3.MDL"], 4, 4);
            FileSystem.CopyFile([edtEnemyModel, "KAGE3.MDO"], [pubEnemyModel, "KAGE3.MDO"], 4, 4);
        }

        /// <summary>
        /// Compiles npc data
        /// </summary>
        void CompileNPCs(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling NPCs...");

            // Get the editor npc data directories
            string edtNpc        = Path.Combine(args.InstancePath, "DATA", "NPC");
            string edtNpcModel   = Path.Combine(edtNpc, "MODEL");
            string edtNpcTexture = Path.Combine(edtNpc, "TEXTURE");

            if (!Directory.Exists(edtNpc) || !Directory.Exists(edtNpcModel))
                throw new Exception("Editor NPC data is missing.");

            // Open up NPC.PR2 to find out which npcs we need
            if (!FileFormatNpcPR2.LoadFromFile(Path.Combine(args.PublishPath, "PARAM", "NPC.PR2"), out FileFormatNpcPR2 pr2))
                throw new Exception("npc.pr2 is either missing or corrupt.");

            // Create publish directory(s)
            string pubNpc        = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "NPC"), 2);
            string pubNpcModel   = FileSystem.CreateDirectory(Path.Combine(pubNpc, "MODEL"));
            string pubNpcTexture = FileSystem.CreateDirectory(Path.Combine(pubNpc, "TEXTURE"));

            foreach (NpcPRF npcPRF in pr2.Profiles)
            {
                // Base Model
                if (!FileSystem.CopyFile([edtNpcModel, npcPRF.ModelFile], [pubNpcModel, npcPRF.ModelFile], 4, 4))
                    continue;

                // Control Points
                string itemCP = Path.ChangeExtension(npcPRF.ModelFile, "cp");

                if (FileSystem.FileExists([edtNpcModel, itemCP]))
                    FileSystem.CopyFile([edtNpcModel, itemCP], [pubNpcModel, itemCP], 4, 4);

                // External Texture
                if (npcPRF.useExternalTexture != 0)
                    FileSystem.CopyFile([edtNpcTexture, npcPRF.TextureFile], [pubNpcTexture, npcPRF.TextureFile], 4, 4);
            }
        }

        /// <summary>
        /// Compiles arm data
        /// </summary>
        void CompileArm(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling arms...");

            // Get the editor arm data directories
            string edtArm = Path.Combine(args.InstancePath, "DATA", "MY", "MODEL");

            if (!Directory.Exists(edtArm))
                throw new Exception("Editor arm data is missing.");

            // Create publish directory(s)
            string pubArm = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "MY", "MODEL"));

            FileSystem.CopyFile([edtArm, "ARM.MDL"], [pubArm, "ARM.MDL"], 4, 4);
            FileSystem.CopyFile([edtArm, "ARM.CP"], [pubArm, "ARM.CP"], 4, 4);
        }

        /// <summary>
        /// Compiles map data
        /// </summary>
        void CompileMaps(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling maps...");

            // Get the project map data directories
            string prjMap = Path.Combine(args.ProjectPath, "DATA", "MAP");

            if (!Directory.Exists(prjMap))
                throw new Exception("Project map data is missing.");

            // Get the editor map data directories
            string edtMap        = Path.Combine(args.InstancePath, "DATA", "MAP");
            string edtMapModel   = Path.Combine(edtMap, "MODEL");
            string edtMapTexture = Path.Combine(edtMap, "TEXTURE");

            if (!Directory.Exists(edtMap) || !Directory.Exists(edtMapModel) || !Directory.Exists(edtMapTexture))
                throw new Exception("Editor map data is missing.");

            // Get tool paths required
            string edtMapComp = Path.Combine(args.InstancePath, "TOOL", "MAPCOMP.exe");

            if (!File.Exists(edtMapComp))
                throw new Exception("Instance map compiler is missing!");

            // Create publish directory(s)
            string pubMap        = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "MAP"), 2);
            string pubMapModel   = FileSystem.CreateDirectory(Path.Combine(pubMap, "MODEL"));
            string pubMapTexture = FileSystem.CreateDirectory(Path.Combine(pubMap, "TEXTURE"));

            // Compile MAP -> MPX
            foreach (FileInfo mapFile in new DirectoryInfo(prjMap).EnumerateFiles("*.map"))
            {
                Process compileProcess = Process.Start(edtMapComp,
                [
                    mapFile.FullName,
                    Path.Combine(pubMap, $"{Path.GetFileNameWithoutExtension(mapFile.Name)}.MPX"),
                    args.InstancePath,
                    args.ProjectPath
                ]);

                compileProcess.WaitForExit();
            }

            // Copy EVT
            foreach (FileInfo evtFile in new DirectoryInfo(prjMap).EnumerateFiles("*.evt"))
                FileSystem.CopyFile(evtFile.FullName, Path.Combine(pubMap, evtFile.Name), 3, 3);

            // Copy TXR
            foreach (FileInfo txrFile in new DirectoryInfo(edtMapTexture).EnumerateFiles("*.txr"))
                FileSystem.CopyFile(txrFile.FullName, Path.Combine(pubMapTexture, txrFile.Name), 4, 4);

            // Copy MDO
            foreach (FileInfo mdoFile in new DirectoryInfo(edtMapModel).EnumerateFiles("*.mdo"))
            {
                // Base Model
                FileSystem.CopyFile(mdoFile.FullName, Path.Combine(pubMapModel, mdoFile.Name), 4, 4);

                // Textures
                foreach (string textureFile in FindTexturesForMapModel(mdoFile.FullName))
                    FileSystem.CopyFile([edtMapModel, textureFile], [pubMapModel, textureFile], 4, 4);
            }
        }

        /// <summary>
        /// Finds textures for an map model
        /// </summary>
        string[] FindTexturesForMapModel(string mapModelFile)
        {
            string[] textureFiles = Array.Empty<string>();

            switch (Path.GetExtension(mapModelFile).ToUpperInvariant())
            {
                case ".MDO":
                    if (FileFormatMDO.LoadFromFile(mapModelFile, out FileFormatMDO mdo))
                        textureFiles = mdo.Textures;
                    break;
            }

            return textureFiles;
        }

        /// <summary>
        /// Compiles effect data
        /// </summary>
        void CompileEffects(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling effects...");

            // Get the editor sfx data directories
            string edtSfx = Path.Combine(args.InstancePath, "DATA", "SFX");

            if (!Directory.Exists(edtSfx))
                throw new Exception("Editor SFX data is missing.");

            // Copy entire sfx directory
            FileSystem.CopyDirectory(edtSfx, Path.Combine(args.PublishPath, "DATA", "SFX"));
        }

        /// <summary>
        /// Compiles sound data
        /// </summary>
        void CompileSounds(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling sounds...");

            // get the editor sound data directory 
            string edtSound = Path.Combine(args.InstancePath, "DATA", "SOUND", "SE");

            if (!Directory.Exists(edtSound))
                throw new Exception("Editor sound data is missing.");

            // Create publish directory(s)
            string pubSound = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "SOUND", "SE"), 3);

            // Copy sound data
            foreach (FileInfo sndFile in (new DirectoryInfo(edtSound)).EnumerateFiles("*.snd"))
                FileSystem.CopyFile(sndFile.FullName, Path.Combine(pubSound, sndFile.Name), 4, 4);

            SendStatusUpdate("Compiling music...");

            // get the project music data directory
            string prjMusic = Path.Combine(args.ProjectPath, "DATA", "BGM");

            if (!Directory.Exists(prjMusic))
                throw new Exception("Project music data is missing.");

            // Copy music data
            FileSystem.CopyDirectory(prjMusic, Path.Combine(args.PublishPath, "DATA", "SOUND", "BGM"), 2, 3);
        }

        /// <summary>
        /// Compiles menu data
        /// </summary>
        void CompileMenus(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling menus...");

            // get the editor menu data directory 
            string edtMenu = Path.Combine(args.InstancePath, "DATA", "MENU");

            if (!Directory.Exists(edtMenu))
                throw new Exception("Editor menu data is missing.");

            FileSystem.CopyDirectory(edtMenu, Path.Combine(args.PublishPath, "DATA", "MENU"), 2, 2);
        }

        /// <summary>
        /// Compiles picture data
        /// </summary>
        void CompilePictures(GeneratorStartArgs args)
        {
            SendStatusUpdate("Compiling pictures...");

            // get the project picture data directory
            string prjPicture = Path.Combine(args.ProjectPath, "DATA", "PICTURE");

            if (!Directory.Exists(prjPicture))
                throw new Exception("Project picture data is missing.");

            // Create publish directory(s)
            string pubPicture = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "PICTURE"));

            foreach (FileInfo bmpFile in new DirectoryInfo(prjPicture).EnumerateFiles("*.bmp"))
                FileSystem.CopyFile(bmpFile.FullName, Path.Combine(pubPicture, bmpFile.Name), 3, 3);
        }

        /// <summary>
        /// Compiles movie data
        /// </summary>
        void CompileMovies(GeneratorStartArgs args)
        {
            if (((bool)transcodeMovies.Value))
                SendStatusUpdate("Compiling movies (transcode)...");
            else
                SendStatusUpdate("Compiling movies (copy)...");

            // get the project movie data directory
            string prjMovie = Path.Combine(args.ProjectPath, "DATA", "MOVIE");

            if (!Directory.Exists(prjMovie))
                throw new Exception("Project movie data is missing.");

            // Create publish directory(s)
            string pubMovie = FileSystem.CreateDirectory(Path.Combine(args.PublishPath, "DATA", "MOVIE"));

            if (((bool)transcodeMovies.Value))
            {
                // Get tool paths required
                string progFFMPEG = Path.Combine(ProgramPath, "TOOLS", "FFMPEG.exe");

                // via transcode - could likely be multithreaded...
                foreach (FileInfo aviFile in new DirectoryInfo(prjMovie).EnumerateFiles("*.avi"))
                {
                    Process transcodeProcess = Process.Start(progFFMPEG,
                    [
                        "-y", "-loglevel", "warning", "-stats",                     // Force overwrite, warnings + errors + encoding status
                                    "-i", $"{aviFile}",                             // Input
                                    "-c:v", "wmv2",                                 // Use Microsoft MPEG4 (wmv) codec for video
                                    "-b:v", "2048k",                                // HUGE Bit Rate because fukkit man
                                    "-c:a", "pcm_s16le",                            // Use PCM 16-Bit signed for audio
                                    $"{Path.Combine(pubMovie, aviFile.Name)}"       // Output
                    ]);

                    transcodeProcess.WaitForExit();
                }
            } 
            else
            {
                // direct copy
                foreach (FileInfo aviFile in new DirectoryInfo(prjMovie).EnumerateFiles("*.avi"))
                    FileSystem.CopyFile(aviFile.FullName, Path.Combine(pubMovie, aviFile.Name), 3, 3);
            }
        }

        /// <summary>
        /// Compiles the vanilla runtime
        /// </summary>
        void CompileVanillaRuntime(GeneratorStartArgs args)
        {
            SendStatusUpdate("Bundling vanilla runtime...");

            // PROJECT.DAT file - weird checksum
            int antiTamperAccumulator = 0;
                antiTamperAccumulator += File.ReadAllBytes(Path.Combine(args.PublishPath, "PARAM", "ENEMY.PR2")).Length;
                antiTamperAccumulator += File.ReadAllBytes(Path.Combine(args.PublishPath, "PARAM", "NPC.PR2")).Length;

            FileSystem.CreateFile(Path.Combine(args.PublishPath, "PROJECT.DAT"), BitConverter.GetBytes(antiTamperAccumulator), 1);

            // Generate the safe file name for the exe and ini
            string safeProjectName = GenerateSafeFileName(args.ProjectName);

            // Configuration File - Use regular copy since we don't want special casing considerations
            File.Copy(Path.Combine(args.ProjectPath, "SOM_DB.INI"), Path.Combine(args.PublishPath, $"{safeProjectName}.INI"), true);

            // Executable clean up
            string exeFile = Path.Combine(args.PublishPath, $"{safeProjectName}.EXE");
            File.Copy(Path.Combine(args.InstancePath, "TOOL", "SOM_RT.exe"), exeFile, true);

            // Patch executable data
            string progRCEDIT = Path.Combine(ProgramPath, "TOOLS", "RCEDIT.EXE");

            Process resourceEditProcess = Process.Start(progRCEDIT,
                [
                    $"{exeFile}",                                                                               // Executable
                    "--set-icon", $"{Path.Combine(args.ProjectPath, "project.ico")}",                           // Set Icon
                    "--set-version-string", "FileDescription",      $"Sword of Moonlight Game",
                    "--set-version-string", "LegalCopyright",       $"©{DateTime.Now.Year} {args.AuthorName}",  // Set Copyright
                    "--set-version-string", "OriginalFilename",     $"{safeProjectName}.exe",                   // Set Original Filename
                    "--set-version-string", "ProductName",          $"{args.ProjectName}",                      // Set Product Name
                    "--set-version-string", "FileVersion",          $"1.0.0.0",
                    "--set-version-string", "ProductVersion",       $"1.0.0.0",
                    "--set-version-string", "CompanyName",          $"{args.AuthorName}"
                ]);
            resourceEditProcess.WaitForExit();

            // Generate correct executable checksum
            Process.Start(Path.Combine(ProgramPath, "TOOLS", "PECHKSUM.EXE"), ["/nologo", exeFile]).WaitForExit();
        }

        /// <summary>
        /// Compiles the ex runtime
        /// </summary>
        void CompileExRuntime(GeneratorStartArgs args)
        {
            SendStatusUpdate("Bundling ex runtime...");

            // First we need to get the path to the SomEx bundles
            string somExBundlePath = Path.Combine(ProgramPath, "Runtime", "SomExRuntime");

            // Now we can see if our specific bundle exists...
            string somExBundle = Path.Combine(somExBundlePath, $"{((SomExVersion)somExVersion.Value)}.ZIP");

            if (!File.Exists(somExBundle))
                throw new Exception($"SomEx Version '{((SomExVersion)somExVersion.Value)}' bundle is missing!");

            // We need to install the bundle to the publish path
            ZipFile.ExtractToDirectory(somExBundle, args.PublishPath, true);

            // Now we need to rename the assets
            string safeProjectName = $"{GenerateSafeFileName(args.ProjectName)}Ex";

            // INI File
            FileSystem.RenameFile(Path.Combine(args.PublishPath, "EX_RT.INI"), $"{safeProjectName}.INI", 1);

            // SOM File
            FileSystem.CreateFile(Path.Combine(args.PublishPath, $"{safeProjectName}.SOM"), ["EX=EX.INI", $"ICON={safeProjectName}.EXE"], 1, true);

            // EXE File
            FileSystem.RenameFile(Path.Combine(args.PublishPath, "EX_RT.EXE"), $"{safeProjectName}.EXE", 1);

            string exeFile = Path.Combine(args.PublishPath, $"{safeProjectName}.EXE");

            // Patch executable data
            string progRCEDIT = Path.Combine(ProgramPath, "TOOLS", "RCEDIT.EXE");

            Process resourceEditProcess = Process.Start(progRCEDIT,
                [
                    $"{exeFile}",                                                                               // Executable
                    "--set-icon", $"{Path.Combine(args.ProjectPath, "project.ico")}",                           // Set Icon
                    "--set-version-string", "FileDescription",      $"Sword of Moonlight Ex Game",
                    "--set-version-string", "LegalCopyright",       $"©{DateTime.Now.Year} {args.AuthorName}",  // Set Copyright
                    "--set-version-string", "OriginalFilename",     $"{safeProjectName}.exe",                   // Set Original Filename
                    "--set-version-string", "ProductName",          $"{args.ProjectName}",                      // Set Product Name
                    "--set-version-string", "FileVersion",          $"1.0.0.0",
                    "--set-version-string", "ProductVersion",       $"1.0.0.0",
                    "--set-version-string", "CompanyName",          $"{args.AuthorName}"
                ]);
            resourceEditProcess.WaitForExit();

            // Generate correct executable checksum
            Process.Start(Path.Combine(ProgramPath, "TOOLS", "PECHKSUM.EXE"), ["/nologo", exeFile]).WaitForExit();
        }

        /// <summary>
        /// Sends a status update to Lawful Blade
        /// </summary>
        void SendStatusUpdate(string statusUpdate) =>
            UpdateStatus?.Invoke(statusUpdate);

        /// <summary>
        /// Copies assets which are named with the same format as an original file, but with '_n' before the file extension.
        /// </summary>
        void CopyAdditiveAssets(string baseSource, string baseTarget, string filename, int sourceLogLength = 3, int targetLogLength = 3)
        {
            // Parts
            string nameNoExt = Path.GetFileNameWithoutExtension(filename);
            string ext       = Path.GetExtension(filename);

            // State
            int additiveID      = 0;
            string additiveName = string.Empty;

            do
            {
                additiveName = $"{nameNoExt}_{additiveID}{ext}";

                if (!File.Exists(Path.Combine(baseSource, additiveName)))
                    break;

                FileSystem.CopyFile([baseSource, additiveName], [baseTarget, additiveName], sourceLogLength, targetLogLength);

                // Increment additive ID
                additiveID++;
            } while (true);
        }


        /// <summary>
        /// Generates a safe filename name from an input string
        /// </summary>
        string GenerateSafeFileName(string filename)
        {
            // Remove invalid characters
            var invalidChars = Path.GetInvalidFileNameChars();
            filename = new string([.. filename.Where(ch => !invalidChars.Contains(ch))]);

            // Spaces -> Underscores
            filename = filename.Replace(' ', '_');

            // Remove ending '.' or ' '
            filename = filename.TrimEnd(' ', '.');

            return filename;
        }
    }
}