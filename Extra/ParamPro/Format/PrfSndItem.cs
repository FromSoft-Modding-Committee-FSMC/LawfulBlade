using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParamPro.Format
{
    public class PrfSndItem
    {
        public byte delay;      // in frames
        public sbyte pitch;     // play back pitch
        public short id;        // ID of the sound
    }
}