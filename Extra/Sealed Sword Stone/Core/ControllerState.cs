using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sealed_Sword_Stone.Core
{
    public class ControllerState
    {
        public bool[] ButtonStates { get; private set; } = new bool[256];
        public bool[] HatValues { get; private set; }    = new bool[8];

        public bool GetAnyHatPressed(out int hatID)
        {
            for (int i = 0; i < HatValues.Length; ++i)
            {
                if (!HatValues[i])
                    continue;

                hatID = i;
                return true;
            }

            hatID = -1;
            return false;
        }

        public bool GetAnyButtonPressed(out int buttonID)
        {
            for (int i = 0; i < ButtonStates.Length; ++i)
            {
                if (!ButtonStates[i])
                    continue;

                buttonID = i;
                return true;
            }

            buttonID = -1;
            return false;
        }
    }
}
