namespace LawfulBladeManager.Tagging
{
    public class Tags
    {
        /// <summary>
        /// Use the first two characters to generate a background colour from the tag
        /// </summary>
        /// <param name="tag">The text of the tag</param>
        /// <returns>A colour</returns>
        public static Color MakeBackgroundColour(string tag)
        {
            // This function takes the first and second letter,
            // looks up a colour for each and then blends them together.

            // Red    (A, B, C, D)      1: #E74C3C, 2: #A93226
            // Green  (E, F, G, H)      1: #2ECC71, 2: #229954
            // Blue   (I, J, K, L)      1: #3498DB, 2: #2471A3
            // Yellow (M, N, O, P)      1: #F1C40F, 2: #D68910
            // Pink   (Q, R, S, T)      1: #8E44AD, 2: #5B2C6F
            // Cyan   (U, V, W, X)      1: #1ABC9C, 2: #138D75
            // Grey   (Y, Z)            1: #BDC3C7, 2: #707B7C

            // When the text is shorter than two characters, return aqua
            if (tag.Length < 2)
                return Color.Aqua;

            // Force tag into uppercase
            tag = tag.ToUpper();

            // First character colour
            Color C1 = tag[0] switch
            {
                (>= 'A') and (<= 'D') => HexC(0xE74C3C),
                (>= 'E') and (<= 'H') => HexC(0x2ECC71),
                (>= 'I') and (<= 'L') => HexC(0x3498DB),
                (>= 'M') and (<= 'P') => HexC(0xF1C40F),
                (>= 'Q') and (<= 'T') => HexC(0x8E44AD),
                (>= 'U') and (<= 'X') => HexC(0x1ABC9C),
                _ => HexC(0xBDC3C7)
            };

            // Second character colour
            Color C2 = tag[1] switch
            {
                (>= 'A') and (<= 'D') => HexC(0xA93226),
                (>= 'E') and (<= 'H') => HexC(0x229954),
                (>= 'I') and (<= 'L') => HexC(0x2471A3),
                (>= 'M') and (<= 'P') => HexC(0xD68910),
                (>= 'Q') and (<= 'T') => HexC(0x5B2C6F),
                (>= 'U') and (<= 'X') => HexC(0x138D75),
                _ => HexC(0x707B7C)
            };

            // Blend them
            return Color.FromArgb(0xFF, (int)((C1.R + C2.R) * 0.5F), (int)((C1.G + C2.G) * 0.5F), (int)((C1.B + C2.B) * 0.5f));
        }

        /// <summary>
        /// Use the first two characters to generate a foreground colour from the tag
        /// </summary>
        /// <param name="tag">The text of the tag</param>
        /// <returns>A colour</returns>
        public static Color MakeForegroundColour(string tag)
        {
            Color result = MakeBackgroundColour(tag);

            int b = (int)(255f * result.GetSaturation() + result.GetBrightness()) / 2;

            return Color.FromArgb(b, b, b);
        }

        // Fuck you in the arse Microsoft (why private uint hex colour function?..)
        static Color HexC(uint RGB) =>
            Color.FromArgb(0xFF, (int)(RGB >> 16) & 0xFF, (int)(RGB >> 8) & 0xFF, (int)(RGB >> 0) & 0xFF);
    }
}
