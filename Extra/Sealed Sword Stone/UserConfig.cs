using Sealed_Sword_Stone.Core;
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
        // | 0xg1xxxxxx   = Keyboard Device |
        // | 0xg2xxxxxx   = Mouse Device    |
        // | 0x1dxxxxxx   = Gamepad Button  |
        // | 0x2dxxxxxx   = Gamepad Axis    |
        // | 0x3dxxxxxx   = Gamepad Hat     |
        // | 0x00ffffff   = Unbound         |
        // ----------------------------------
        // | 0xDDxxPDKM   = Form.           |
        // |   ^^ = Device Flags            |
        // |        Upper4  = Gamepad       |
        // |        Lowest2 = Keyboard/Mouse|
        // |        Lower   = Alt           |
        // |     ^^ = Game Pad Binding      |
        // |       ^^ = Alt Binding         |
        // |         ^^ = Key/Mouse Binding |
        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        [JsonInclude]
        public uint PlayerMoveForward  { get; set; } = ControlBinding.Pack(3, 1, 0, 0x00, 0x57, 0);       // Default: Keyboard = W, Gamepad = Hat 0

        [JsonInclude]
        public uint PlayerMoveBackward { get; set; } = ControlBinding.Pack(3, 1, 0, 0x04, 0x53, 0);       // Default: Keyboard = S, Gamepad = Hat 4

        [JsonInclude]
        public uint PlayerStrafeLeft   { get; set; } = ControlBinding.Pack(1, 1, 0, 0x04, 0x41, 0);       // Default: Keyboard = A, Gamepad = Button 4

        [JsonInclude]
        public uint PlayerStrafeRight  { get; set; } = ControlBinding.Pack(1, 1, 0, 0x05, 0x44, 0);       // Default: Keyboard = D, Gamepad = Button 5

        [JsonInclude]
        public uint PlayerTurnLeft     { get; set; } = ControlBinding.Pack(3, 0, 0, 0x06, 0xFF, 0xFF);    // Default: Gamepad = Hat 6

        [JsonInclude]
        public uint PlayerTurnRight    { get; set; } = ControlBinding.Pack(3, 0, 0, 0x02, 0xFF, 0xFF);    // Default: Gamepad = Hat 2

        [JsonInclude]
        public uint PlayerLookUp       { get; set; } = ControlBinding.Pack(1, 0, 0, 0x07, 0xFF, 0xFF);    // Default: Gamepad = Button 7

        [JsonInclude]
        public uint PlayerLookDown     { get; set; } = ControlBinding.Pack(1, 0, 0, 0x06, 0xFF, 0xFF);    // Default: Gamepad = Button 6

        [JsonInclude]
        public uint ActionAttack       { get; set; } = ControlBinding.Pack(1, 2, 0, 0x03, 0x00, 0x00);    // Default: Mouse = Left, Gamepad = Button 3

        [JsonInclude]
        public uint ActionCast         { get; set; } = ControlBinding.Pack(1, 2, 0, 0x00, 0x02, 0x00);    // Default: Mouse = Right, Gamepad = Button 0

        [JsonInclude]
        public uint ActionInspect      { get; set; } = ControlBinding.Pack(1, 1, 0, 0x01, 0x45, 0x00);    // Default: Keyboard = E, Gamepad = Button 1

        [JsonInclude]
        public uint ActionSprint       { get; set; } = ControlBinding.Pack(1, 1, 0, 0x01, 0x10, 0x00);    // Default: Keyboard = Shift, Gamepad = Button 1

        [JsonInclude]
        public uint MenuOpen           { get; set; } = ControlBinding.Pack(1, 1, 0, 0x02, 0x09, 0x00);    // Default: Keyboard = Tab, Gamepad = Button 2

        [JsonInclude]
        public uint MenuConfirm        { get; set; } = ControlBinding.Pack(1, 1, 0, 0x01, 0x20, 0x00);    // Default: Keyboard = Space, Gamepad = Button 1 

        [JsonInclude]
        public uint MenuCancel         { get; set; } = ControlBinding.Pack(1, 1, 0, 0x02, 0x1B, 0x00);    // Default: Keyboard = Escape, Gamepad = Button 2

        [JsonInclude]
        public uint MenuUp             { get; set; } = ControlBinding.Pack(3, 1, 1, 0x00, 0x57, 0x26);    // Default: Keyboard = W & Up Arrow, Gamepad = Hat 0

        [JsonInclude]
        public uint MenuDown           { get; set; } = ControlBinding.Pack(3, 1, 1, 0x04, 0x53, 0x28);    // Default: Keyboard = S & Down Arrow, Gamepad = Hat 4

        [JsonInclude]
        public uint MenuLeft           { get; set; } = ControlBinding.Pack(3, 1, 1, 0x06, 0x41, 0x25);    // Default: Keyboard = A & Left Arrow, Gamepad = Hat 6

        [JsonInclude]
        public uint MenuRight          { get; set; } = ControlBinding.Pack(3, 1, 1, 0x02, 0x44, 0x27);    // Default: Keyboard = D & Right Arrow, Gamepad = Hat 2

        // Control Settings
        public bool UseMouseLook { get; set; } = false;

        public float MouseSensitivity { get; set; } = 0.5f;

        public float MouseSmoothing { get; set; } = 0.5f;

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
