using Lawful.IO;
using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using Lawful.Resource;

[CreateAssetMenu(fileName = "GameInformation", menuName = "Lawful Runtime/Game Information")]
public class GameInformation : ScriptableObject
{
    [field: Header("Sequences")]
    [field: SerializeField] public GameSequenceInfo attractSequence { get; private set; }
    [field: SerializeField] public GameSequenceInfo openingSequence { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence1 { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence2 { get; private set; }
    [field: SerializeField] public GameSequenceInfo endingSequence3 { get; private set; }
    [field: SerializeField] public GameSequenceInfo creditsSequence { get; private set; }

    [field: Header("Images")]
    [field: SerializeField] public string titleImage { get; private set; }
    [field: SerializeField] public string closeImage { get; private set; }
    [field: SerializeField] public string menuBackgroundImage { get; private set; }

    [field: Header("Player")]
    [field: SerializeField] public GamePlayerMovementInfo playerMovementInfo { get; private set; }
    [field: SerializeField] public GamePlayerConfiguration playerNormalSetup { get; private set; }
    [field: SerializeField] public GamePlayerConfiguration playerDebugSetup { get; private set; }

    [field: Header("Game")]
    [field: SerializeField] public string[] counterNames { get; private set; }
    [field: SerializeField] public byte startingMapID { get; private set; }
    [field: SerializeField] public ushort[] soundFXIds { get; private set; }

    [field: Header("Data Declarations")]
    [field: SerializeField] public GameClassNameInfo classNameInfo { get; private set; }
    [field: SerializeField] public GameMagicInfo magicTableInfo { get; private set; }
    [field: SerializeField] public GameConfiguration gameConfiguration { get; private set; }
    [field: SerializeField] public string[] gameMessages { get; private set; }
    [field: SerializeField] public string goldSymbol { get; private set; }


    /// <summary>
    /// Imports legacy sys.dat file
    /// </summary>
    /// <param name="path">Path to SOM game sys.dat</param>
    public void ImportFromLegacyFile(string path)
    {
        //
        // WARNING:
        //  Technically all strings are SHIFT-JIS, but we'll cross that bridge when we come to it...
        //

        // Open the legacy file
        using FileInputStream ins = new(path);

        //
        // Sequences, Title & Close Image
        //
        attractSequence = LoadSequence(ins, "TITLE.DAT");
        titleImage      = ins.ReadFixedString(31);
        openingSequence = LoadSequence(ins, "OPENNING.DAT");
        endingSequence1 = LoadSequence(ins, "ENDING1.DAT");
        endingSequence2 = LoadSequence(ins, "ENDING2.DAT");
        endingSequence3 = LoadSequence(ins, "ENDING3.DAT");
        creditsSequence = LoadSequence(ins, "STAFF.DAT");
        closeImage      = ins.ReadFixedString(31);

        // Player Movement
        GamePlayerMovementInfo playerMovementInfoTemp = new ();
        playerMovementInfoTemp.runningEnabled = (ins.ReadU8() != 0);
        ins.ReadU8();   // Unknown
        playerMovementInfoTemp.walkingSpeed = ins.ReadF32();
        playerMovementInfoTemp.runningSpeed = ins.ReadF32();
        playerMovementInfoTemp.turningSpeed = (ins.ReadU16() / 360f);   // This is in degrees per second, so we do a quick conversion here to get a 0f - 1f mapping
        ins.ReadU8();   // Unknown
        playerMovementInfo = playerMovementInfoTemp;

        // Class Names
        GameClassNameInfo classNameInfoTemp = new();
        classNameInfoTemp.classNames = new string[25];
        for (int i = 0; i < 25; ++i)
            classNameInfoTemp.classNames[i] = ins.ReadFixedString(15);
        classNameInfoTemp.classNameMag = ins.ReadU16Array(4);
        classNameInfoTemp.classNameStr = ins.ReadU16Array(4);
        classNameInfo = classNameInfoTemp;

        // Player Magic Table
        GameMagicInfo magicInfoTemp = new();
        magicInfoTemp.magicClassIDs = ins.ReadS8Array(32);
        magicInfoTemp.levelRequirements = ins.ReadU8Array(32);
        magicTableInfo = magicInfoTemp;

        // System Configuration
        gameConfiguration = new GameConfiguration
        {
            allowSaveInMenu        = (ins.ReadU8() != 0),
            forbidOverencumberance = (ins.ReadU8() != 0),
            compassSkin = ins.ReadU8(),
            gaugeSkin = ins.ReadU8(),
            u8x4 = ins.ReadU8(),
            menuSkin = ins.ReadU8()
        };

        // System Messages #1
        List<string> messageList = new();
        for(int i = 0; i < 13; ++i) // 13 are stored now, 3 more are stored later.
            messageList.Add(ins.ReadFixedString(41));

        // Gold Symbol
        goldSymbol = ins.ReadFixedString(3);

        // Player Setups (Normal, Debug)
        playerNormalSetup = new GamePlayerConfiguration
        {
            startStrength = ins.ReadU16(),
            startMagic = ins.ReadU16(),
            startMaxHP = ins.ReadU16(),
            startMaxMP = ins.ReadU16(),
            startGold = ins.ReadU32(),
            startExperience = ins.ReadU32(),
            startLevel = ins.ReadU8(),
            startEquipment = ins.ReadS8Array(8),
            startInventory = ins.ReadU8Array(251)
        };
        playerDebugSetup = new GamePlayerConfiguration
        {
            startStrength = ins.ReadU16(),
            startMagic = ins.ReadU16(),
            startMaxHP = ins.ReadU16(),
            startMaxMP = ins.ReadU16(),
            startGold = ins.ReadU32(),
            startExperience = ins.ReadU32(),
            startLevel = ins.ReadU8(),
            startEquipment = ins.ReadS8Array(8),
            startInventory = ins.ReadU8Array(251)
        };

        // Starting map
        startingMapID = ins.ReadU8();

        // Counters
        counterNames = new string[1024];
        for (int i = 0; i < 1024; ++i)
            counterNames[i] = ins.ReadFixedString(31);

        ins.ReadU8();   // u8x8319;

        // Sound Configuration
        soundFXIds = ins.ReadU16Array(16);

        // Menu Background File
        menuBackgroundImage = ins.ReadFixedString(31);

        // Some unknown crap
        ins.ReadU8Array(6); // u8x8383

        // System Messages #2
        for (int i = 0; i < 3; ++i) // these are the other 3 messages
            messageList.Add(ins.ReadFixedString(41));

        // More unknown crap
        ins.ReadU8Array(5); // u8x83db

        // Store our accumulated list of messages (damn you, FromSoftware!)
        gameMessages = messageList.ToArray();
    }

    /// <summary>
    /// Loads sequence information from the SYS.DAT file
    /// </summary>
    /// <param name="br">A BinaryReader to read from</param>
    /// <param name="slideshowSource">The source of the slide show if this is not a movie or null</param>
    /// <returns></returns>
    GameSequenceInfo LoadSequence(FileInputStream ins, string slideshowSource)
    {
        GameSequenceInfo temp = new()
        {
            mode = (SequenceMode)ins.ReadU8(),
            file = ins.ReadFixedString(31)
        };

        // Override with slideshow file if mode is for a slideshow
        switch(temp.mode)
        {
            case SequenceMode.Video:
                temp.file = Path.Combine(ResourceManager.GamePath, "DATA", "MOVIE", temp.file);
                break;

            case SequenceMode.Slideshow:
                temp.file = Path.Combine(ResourceManager.GamePath, "PARAM", slideshowSource);
                break;
        }

        return temp;
    }
}

[Serializable]
public struct GameSequenceInfo
{
    public SequenceMode mode;
    public string file;
}

[Serializable]
public struct GamePlayerMovementInfo
{
    public bool runningEnabled;
    public float walkingSpeed;
    public float runningSpeed;
    public float turningSpeed; // degrees per second
}

[Serializable]
public struct GameClassNameInfo
{
    public string[] classNames;
    public ushort[] classNameMag;
    public ushort[] classNameStr;
}

[Serializable]
public struct GameMagicInfo
{
    public sbyte[] magicClassIDs;
    public byte[] levelRequirements;
}

[Serializable]
public struct GameConfiguration
{
    public bool allowSaveInMenu;
    public bool forbidOverencumberance;
    public byte compassSkin;
    public byte gaugeSkin;
    public byte u8x4;
    public byte menuSkin;
};

[Serializable]
public struct GamePlayerConfiguration
{
    public ushort startStrength;
    public ushort startMagic;
    public ushort startMaxHP;
    public ushort startMaxMP;
    public uint startGold;
    public uint startExperience;
    public byte startLevel;
    public sbyte[] startEquipment;
    public byte[] startInventory;
}