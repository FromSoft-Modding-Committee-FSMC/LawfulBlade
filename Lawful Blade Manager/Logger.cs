namespace LawfulBladeManager
{
    public static class Logger
    {
        private static readonly string logHeadingStart = "<".Colourize(0x444444) + "[".Colourize(0xAAAAAA);
        private static readonly string logHeadingEnd = "]".Colourize(0xAAAAAA) + ">".Colourize(0x444444) + ": ".Colourize(0x444444);
        private static readonly string logHeadingCenter = "]".Colourize(0xAAAAAA) + "-".Colourize(0x888888) + "[".Colourize(0xAAAAAA);

        public static void Write(string heading, uint headingColour, string message)
        {
            DateTime dt = DateTime.Now;

            Console.WriteLine(string.Join("",
                logHeadingStart,
                $"{dt.Day:D2}:{dt.Month:D2}:{dt.Year:D4}".Colourize(0xCCCCCC),
                logHeadingCenter,
                $"{dt.Hour:D2}:{dt.Minute:D2}:{dt.Second:D2}",
                logHeadingCenter,
                heading.Colourize(headingColour),
                logHeadingEnd,
                message.Colourize(0xFFFFFF)
                ));
        }

        public static void ShortWrite(string heading, uint headingColour, string message) =>
            Console.WriteLine(string.Join("", logHeadingStart, heading.Colourize(headingColour), logHeadingEnd, message.Colourize(0xFFFFFF)));

        public static void Info(string message) =>
            Write("INFO", 0x88CCFF, message);

        public static void Warn(string message) =>
            Write("WARN", 0xFFFF88, message);

        public static void Error(string message) =>
            Write("SHIT", 0xFF8888, message);

        public static void ShortInfo(string message) =>
            ShortWrite("INFO", 0x88CCFF, message);

        public static void ShortWarn(string message) =>
            ShortWrite("WARN", 0xFFFF88, message);

        public static void ShortError(string message) =>
            ShortWrite("SHIT", 0xFF8888, message);

        public static string Colourize(this string input, uint colour) =>
            $"\u001b[38;2;{(colour >> 16) & 0xFF};{(colour >> 8) & 0xFF};{(colour >> 0) & 0xFF}m{input}\u001b[0m";
    }
}
