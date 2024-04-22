using System.Text.Json.Serialization;

namespace LawfulBladeManager.Tagging
{
    public struct Tag
    {
        [JsonInclude]
        public string Text;

        [JsonIgnore]
        public Color BackgroundColour => GetColourByName();

        [JsonIgnore]
        public Color ForegroundColour 
        {
            get
            {
                int b = (int)(255f * BackgroundColour.GetSaturation() + BackgroundColour.GetBrightness()) / 2;

                return Color.FromArgb(b, b, b);
            }
        }

        Color GetColourByName()
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

            // When the text is shorter than two characters, return basic grey
            if (Text.Length < 2)
                return Color.Aqua;

            // Blend a colour
            Color C1, C2;

            string ustr = Text.ToUpper();

            // First Character Colour (this is so bad)
            if (ustr[0] >= 'A' && ustr[0] <= 'D')
                C1 = HexC(0xE74C3C);
            else
            if (ustr[0] >= 'E' && ustr[0] <= 'H')
                C1 = HexC(0x2ECC71);
            else
            if (ustr[0] >= 'I' && ustr[0] <= 'L')
                C1 = HexC(0x3498DB);
            else
            if (ustr[0] >= 'M' && ustr[0] <= 'P')
                C1 = HexC(0xF1C40F);
            else
            if (ustr[0] >= 'Q' && ustr[0] <= 'T')
                C1 = HexC(0x8E44AD);
            else
            if (ustr[0] >= 'U' && ustr[0] <= 'X')
                C1 = HexC(0x1ABC9C);
            else
                C1 = HexC(0xBDC3C7);

            // Second character colour (it just gets worse eh?)
            if (ustr[1] >= 'A' && ustr[1] <= 'D')
                C2 = HexC(0xA93226);
            else
            if (ustr[1] >= 'E' && ustr[1] <= 'H')
                C2 = HexC(0x229954);
            else
            if (ustr[1] >= 'I' && ustr[1] <= 'L')
                C2 = HexC(0x2471A3);
            else
            if (ustr[1] >= 'M' && ustr[1] <= 'P')
                C2 = HexC(0xD68910);
            else
            if (ustr[1] >= 'Q' && ustr[1] <= 'T')
                C2 = HexC(0x5B2C6F);
            else
            if (ustr[1] >= 'U' && ustr[1] <= 'X')
                C2 = HexC(0x138D75);
            else
                C2 = HexC(0x707B7C);

            // Blend them
            return Color.FromArgb(0xFF, (int)((C1.R + C2.R) * 0.5F), (int)((C1.G + C2.G) * 0.5F), (int)((C1.B + C2.B) * 0.5f));
        }

        // Fuck you in the arse Microsoft (why private uint hex colour function?..)
        static Color HexC(uint RGB) =>
            Color.FromArgb(0xFF, (int)(RGB >> 16) & 0xFF, (int)(RGB >> 8) & 0xFF, (int)(RGB >> 0) & 0xFF);
    }
}
