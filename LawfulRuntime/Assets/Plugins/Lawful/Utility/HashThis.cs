using System.Runtime.CompilerServices;
using System.Text;

namespace Lawful.Utility
{
	/// <summary>
	/// HashThis allows quick and easy hashing of strings and byte buffers
	/// </summary>
	public static class HashThis
	{
		/// <summary>
		/// BaseHash64 is a replacable root function for hashing our various types of data.
		/// </summary>
		/// <param name="buffer">The data to hash</param>
		/// <returns>A 64-bit hash depending on implementation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ulong BaseHash64(byte[] buffer)
        {
			// Default 64-bit hash implementation is FNV-1a, based on the implementation here: https://github.com/jslicer/FNV-1a/blob/master/Fnv1a/Fnv1a64.cs
			const ulong FNV1A_64_OFFSET = 0xCBF29CE484222325;
			const ulong FNV1A_64_PRIME  = 0x100000001B3;
			
			// I don't give a shit about overflows... SPEED... SPEEEEEEEED
			unchecked
            {
				ulong hash = FNV1A_64_OFFSET;

				foreach (byte b in buffer)
				{
					hash ^= b;
					hash *= FNV1A_64_PRIME;

				}

				// Return the hash
				return hash;
			}
		}

		/// <summary>
		/// Gets a 64-bit hash of a string
		/// </summary>
		/// <param name="stringToHash">The string you wish to hash</param>
		/// <returns>The hash dummy</returns>
		public static ulong StringTo64(string stringToHash) =>
			BaseHash64(Encoding.UTF8.GetBytes(stringToHash));
	}
}

