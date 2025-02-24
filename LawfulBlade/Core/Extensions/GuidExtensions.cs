using System.Globalization;

namespace LawfulBlade.Core.Extensions
{
    /// <summary>
    /// Really crappy generator for Guids
    /// </summary>
    public static class GuidExtensions
    {
        public static Guid GenerateGuid(string packageName, string authorName, DateTime date)
        {
            // We need a 32-bit hash of the package name... I understand this sucks, but who cares, right?..
            uint nameHash = 0x20304050;
            for (int i = 0; i < packageName.Length; ++i)
                nameHash ^= (uint)(packageName[i] << (8 * (i % 3)));

            // And a 32-bit hash of the author name. I call this algorithm crap hash
            uint authorHash = 0x32649616;
            for (int i = 0; i < authorName.Length; ++i)
                authorHash ^= (uint)(authorName[i] << (8 * (i % 3)));

            // Now we can finally return a Guid
            return new Guid(
                (uint)nameHash,                         // 32-Bit Hash of Package Name
                (ushort)(authorHash & 0xFFFF),          // Lower 16-Bits of Author Name Hash
                (ushort)((authorHash >> 16) & 0xFFFF),  // Upper 16-Bits of Author Name Hash
                (byte)0x1b,                             // Lawful Blade Tag, '1b' looks like 'lb'
                (byte)ISOWeek.GetWeekOfYear(date),
                (byte)date.Day,
                (byte)date.Month,
                (byte)date.DayOfWeek,
                (byte)date.Hour,
                (byte)date.Minute,
                (byte)date.Second);
        }
    }
}
