using System;
using System.IO;
using System.Collections.Generic;

namespace Lawful.IO 
{
    public partial class FileInputStream : IDisposable
    {
        // Constants
        private const int defaultBufferSize = 1024;

        // Data
        readonly Stack<long> jumpStack;
        Stream fstream;
        readonly byte[] buffer;

        /// <summary>The current byte position of the stream.</summary>
        public long Position => fstream.Position;

        /// <summary>The current byte size of the stream.</summary>
        public long Size => fstream.Length;

        /// <summary>
        /// Constructs a new FileInputStream from a file.
        /// </summary>
        /// <param name="filepath">Path to the file</param>
        public FileInputStream(string filepath) : this(File.OpenRead(filepath)) { }

        /// <summary>
        /// Constructs a new FileInputStream from a byte array.
        /// </summary>
        /// <param name="buffer">Buffer to read bytes from</param>
        public FileInputStream(byte[] buffer) : this(new MemoryStream(buffer)) { }

        /// <summary>
        /// Constructs a new FileInputStream from an existing base stream
        /// </summary>
        /// <param name="stream">Some generic stream type.</param>
        /// <exception cref="ArgumentNullException">When stream is null.</exception>
        public FileInputStream(Stream stream)
        {
            jumpStack = new Stack<long>(16);
            fstream = stream ?? throw new ArgumentNullException(nameof(stream));
            buffer = new byte[defaultBufferSize];
        }
    }
}

