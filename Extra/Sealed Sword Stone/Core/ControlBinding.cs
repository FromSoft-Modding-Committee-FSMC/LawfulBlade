namespace Sealed_Sword_Stone.Core
{
    public class ControlBinding
    {
        public uint GamePadBindingType { get; set; } // (0 = None, 1 = Button, 2 = Axis, 3 = Hat)

        public uint DevicesBindingType { get; set; } // (0 = None, 1 = Keyboard, 2 = Mouse)

        public uint DevicesBindingAltType { get; set; } // (0 = None, 1 = Keyboard, 2 = Mouse)

        public uint GamePadInputID { get; set; }     // The ID of the gamepad input

        public uint DevicesInputID { get; set; }     // The ID of the main device input

        public uint DevicesInputAltID { get; set; }  // The ID of the alternate device input

        public uint PackedBinding =>
            ((GamePadBindingType    & 0xF) << 28) |
            ((DevicesBindingType    & 0x3) << 26) |
            ((DevicesBindingAltType & 0x3) << 24) |
            ((GamePadInputID       & 0xFF) << 16) |
            ((DevicesInputAltID    & 0xFF) << 08) |
            ((DevicesInputID       & 0xFF) << 00);

        /// <summary>
        /// Constructor used to create empty bindings
        /// </summary>
        public ControlBinding() : this(0, 0, 0, 0xFF, 0xFF, 0xFF) { }

        /// <summary>
        /// Constructor used to load a control binding from a packed binding
        /// </summary>
        public ControlBinding(uint packedBinding) :
            this(
                (packedBinding & 0xF0000000) >> 28,
                (packedBinding & 0x0C000000) >> 26,
                (packedBinding & 0x03000000) >> 24,
                (packedBinding & 0x00FF0000) >> 16,
                (packedBinding & 0x000000FF) >> 00,
                (packedBinding & 0x0000FF00) >> 08) {}

        /// <summary>
        /// Constructor used to define an already existing binding
        /// </summary>
        public ControlBinding(uint gpInputType, uint kmInputType, uint kmInputAltType, uint gpInputID, uint kmInputID, uint kmInputAltID)
        {
            GamePadBindingType    = gpInputType;
            DevicesBindingType    = kmInputType;
            DevicesBindingAltType = kmInputAltType;
            GamePadInputID        = gpInputID;
            DevicesInputID        = kmInputID;
            DevicesInputAltID     = kmInputAltID;
        }

        public static uint Pack(uint gpInputType, uint kmInputType, uint kmInputAltType, uint gpInputID, uint kmInputID, uint kmInputAltID)
        {
            return ((gpInputType    & 0xF) << 28) |
                   ((kmInputType    & 0x3) << 26) |
                   ((kmInputAltType & 0x3) << 24) |
                   ((gpInputID     & 0xFF) << 16) |
                   ((kmInputID     & 0xFF) << 00) |
                   ((kmInputAltID  & 0xFF) << 08);
        }
    }
}
