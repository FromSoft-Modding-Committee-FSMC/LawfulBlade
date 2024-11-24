using Lawful.IO;
using System;

namespace Lawful.Resource.FileFormat
{
    /// <summary>
    /// File Format Handler is the base class all format handlers must inherit from
    /// </summary>
    public abstract class FileFormatHandler<T>
    {
        /// <summary>Basic metadata of File Format the handler is for</summary>
        public virtual FileFormatMetadata Metadata         => throw new NotImplementedException();

        /// <summary>Basic capabilities of the file format the handler is for</summary>
        public virtual FileFormatCapabilities Capabilities => throw new NotImplementedException();

        /// <summary>
        /// Used when scanning compatible file formats as a second layer of security after extension validation.<br/>
        /// By default it returns <b>false</b> to make sure you read the damn documentation.<br/>
        /// <i>finStream</i> is reused for loading the actual data, so you must not write into this stream.
        /// </summary>
        /// <param name="finStream">The stream file data is provided in</param>
        /// <returns><b>true</b> if the format is validated successfully, and <b>false</b> otherwise</returns>
        public virtual bool Validate(FileInputStream finStream) =>
            false;

        /// <summary>
        /// Parses a file as format from a byte buffer.
        /// </summary>
        /// <param name="finBuffer">The byte buffer file data is provided in</param>
        /// <param name="result">The resulting data from parsing</param>
        /// <returns>True on success, false otherwise.</returns>
        public bool Load(byte[] finBuffer, in T resource)
        {
            // Simply create a file input stream and pass it to the implemented method
            using FileInputStream finStream = new(finBuffer);

            return Load(finStream, resource);
        }

        /// <summary>
        /// Parses a file as format after loading it from a file.
        /// </summary>
        /// <param name="finPath">The path to the file we want to parse</param>
        /// <param name="result">The resulting data from parsing</param>
        /// <returns>True on success, false otherwise.</returns>
        public bool Load(string finPath, in T resource)
        {
            // Simply create a file input stream and pass it to the implemented method
            using FileInputStream finStream = new(finPath);

            return Load(finStream, in resource);
        }

        /// <summary>
        /// Main parsing method for a file.<br/>
        /// Other calling options are provided as wrappers for this one.
        /// </summary>
        /// <param name="finStream">The stream file data is provided in</param>
        /// <param name="result">The resulting data from parsing</param>
        /// <returns>True on success, false otherwise.</returns>
        public virtual bool Load(FileInputStream finStream, in T resource) =>
            throw new NotImplementedException();
    }
}