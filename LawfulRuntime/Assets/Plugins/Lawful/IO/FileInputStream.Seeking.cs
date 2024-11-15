namespace Lawful.IO
{
    public partial class FileInputStream
    {
        /// <summary>
        /// Seeks to an offset relative to the beginning of the stream
        /// </summary>
        /// <param name="offset">Position from start</param>
        /// <returns>The previous position</returns>
        public long SeekBegin(long offset)
        {
            long oldPosition = fstream.Position;
            fstream.Position = offset;
            return oldPosition;
        }

        /// <summary>
        /// Seeks to an offset relative to the end of the stream
        /// </summary>
        /// <param name="offset">Position from end</param>
        /// <returns>The previous position</returns>
        public long SeekEnd(long offset)
        {
            long oldPosition = fstream.Position;
            fstream.Position = (fstream.Length - offset);
            return oldPosition;
        }

        /// <summary>
        /// Seeks to an offset relative to the current position of the stream
        /// </summary>
        /// <param name="offset">Position from the current position</param>
        /// <returns>The previous position</returns>
        public long SeekRelative(long offset)
        {
            long oldPosition = fstream.Position;
            fstream.Position = (oldPosition + offset);
            return oldPosition;
        }

        /// <summary>
        /// Seeks to an offset relative to the beginning of the stream, but adds the old position
        /// to a stack so it can be restored.
        /// </summary>
        /// <param name="offset">Position from start</param>
        /// <returns>The previous position</returns>
        public long Jump(long offset)
        {
            jumpStack.Push(Position);
            fstream.Position = offset;
            return jumpStack.Peek();
        }

        /// <summary>
        /// Returns to the last offset that was jumped from
        /// </summary>
        /// <returns>the previous position</returns>
        public long Return()
        {
            long oldPosition = Position;
            fstream.Position = jumpStack.Pop();
            return oldPosition;
        }
    }
}