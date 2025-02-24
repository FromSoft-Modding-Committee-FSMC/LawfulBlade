using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LawfulBlade.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color Mix(this Color a, Color b, float rate)
            => Color.FromRgb((byte)(a.R * (1F - rate) + b.R * rate), (byte)(a.G * (1F - rate) + b.G * rate), (byte)(a.B * (1F - rate) + b.B * rate));

        public static float GetBrightness(this Color c) =>
          System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetBrightness();

        public static float GetSaturation(this Color c) =>
          System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();
    }
}
