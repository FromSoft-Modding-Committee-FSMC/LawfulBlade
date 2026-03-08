namespace LawfulBladeSDK.IO
{
    /// <summary>
    /// Advanced file writing implementation.
    /// </summary>
    public partial class FileOutputStream : IDisposable
    {
        // Constants
        private const int JUMPSTACK_SIZE = 32;

        // Data
        readonly Stack<long> jumpStack;
        Stream fstream;

        /// <summary>The current byte position of the stream.</summary>
        public long Position => fstream.Position;

        /// <summary>The current byte size of the stream.</summary>
        public long Size => fstream.Length;

        /// <summary>
        /// Constructs a new FileInputStream from a file.
        /// </summary>
        /// <param name="filepath">Path to the file</param>
        public FileOutputStream(string filepath) : this(File.Open(filepath, FileMode.Create)) { }

        /// <summary>
        /// Constructs a new FileInputStream from an existing base stream
        /// </summary>
        /// <param name="stream">Some generic stream type.</param>
        /// <exception cref="ArgumentNullException">When stream is null.</exception>
        public FileOutputStream(Stream stream)
        {
            jumpStack = new Stack<long>(JUMPSTACK_SIZE);
            fstream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
    }
}
