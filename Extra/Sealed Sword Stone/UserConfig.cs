using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sealed_Sword_Stone
{
    public class UserConfig
    {
        // Non Json Properties
        [JsonIgnore]
        static string confPath = Path.Combine(App.ProgramPath, "USER.CONF.SEAL");

        // KEYBOARD-MOUSE-GAMEPAD MAPPING
        // ----------------------------------
        // | 0x01xxxxxx   = Keyboard Device |
        // | 0x02xxxxxx   = Mouse Device    |
        // | 0x00ffffff   = Unbound         |
        // ----------------------------------
        // | 0xDDxxPDKM   = Form.           |
        // |   ^^ = Device Flags            |
        // |        Lower4 = Game Pad ID    |
        // |        Upper4 = Keyboard/Mouse |
        // |     ^^ = Reserved              |
        // |       ^^ = Game Pad Binding    |
        // |         ^^ = Key/Mouse Binding |
        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        [JsonInclude]
        public uint MovePlayerForward { get; set; } = 0x01000057;       // Keyboard, W
        
        [JsonInclude]
        public uint MovePlayerBack    { get; set; } = 0x01000053;       // Keyboard, S

        [JsonInclude]
        public uint MovePlayerRight   { get; set; } = 0x01000044;       // Keyboard, D

        [JsonInclude]
        public uint MovePlayerLeft    { get; set; } = 0x01000041;       // Keyboard, A

        [JsonInclude]
        public uint TurnPlayerRight   { get; set; } = 0x01000045;       // Keyboard, E

        [JsonInclude]
        public uint TurnPlayerLeft    { get; set; } = 0x01000051;       // Keyboard, Q

        [JsonInclude]
        public uint LookPlayerUp      { get; set; } = 0x01000052;       // Keyboard, R

        [JsonInclude]
        public uint LookPlayerDown    { get; set; } = 0x01000046;       // Keyboard, F

        [JsonInclude]
        public uint ActionAttack      { get; set; } = 0x010000A0;       // Keyboard, L Shift

        [JsonInclude]
        public uint ActionMagicCast   { get; set; } = 0x010000A2;       // Keyboard, L Control

        [JsonInclude]
        public uint ActionInspect     { get; set; } = 0x01000020;       // Keyboard, Space

        [JsonInclude]
        public uint ActionOpenMenu    { get; set; } = 0x01000009;       // Keyboard, Tab

        [JsonInclude]
        public uint ActionAcceptMenu  { get; set; } = 0x01000020;       // Keyboard, Space

        [JsonInclude]
        public uint ActionCloseMenu   { get; set; } = 0x0100001B;       // Keyboard, Escape

        // Control Settings
        public bool UseMouseLook      { get; set; } = false;

        /// <summary>
        /// Loads User Config from file
        /// </summary>
        public static UserConfig Load()
        {
            if (!File.Exists(confPath))
                return new UserConfig();

            UserConfig newConfig = JsonSerializer.Deserialize<UserConfig>(File.ReadAllText(confPath)) ??
                new UserConfig();

            return newConfig;
        }

        /// <summary>
        /// Saves user config to file...
        /// </summary>
        public void Save()
        {
            File.WriteAllText(confPath,
                JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true,
                }));
        }
    }
}
