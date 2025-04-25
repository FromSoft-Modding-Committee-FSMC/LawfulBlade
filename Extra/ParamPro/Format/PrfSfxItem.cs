using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParamPro.Format
{
    public class PrfSfxItem
    {
        public byte delay;          // in frames
        public byte controlPoint;   // control point the sfx spawns on
        public short id;            // id of the sfx
    }
}
