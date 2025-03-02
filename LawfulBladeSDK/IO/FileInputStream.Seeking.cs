namespace LawfulBladeSDK.IO
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

        /// <summary>
        /// Returns to the offset that was jumped from at a certain depth (1 = last position, 2 = position before last position)
        /// </summary>
        /// <returns>the previous position</returns>
        public long Return(int depth)
        {
            long oldPosition = Position;

            long newPosition = 0;
            while(depth > 0)
            {
                newPosition = jumpStack.Pop();
                depth--;
            }

            fstream.Position = newPosition;

            return oldPosition;
        }

        /// <summary>
        /// Checks if we are at the end of the stream
        /// </summary>
        /// <returns>Returns true if we are at the end of the stream, and false otherwise</returns>
        public bool EndOfStream() =>
            Position == Size;

        /// <summary>
        /// Aligns the filestream to the next multiple of alignment.
        /// </summary>
        /// <param name="alignment">4, 8, 16 etc...</param>
        public void Align(int alignment) =>
            fstream.Position = (fstream.Position + (alignment - 1)) / alignment * alignment;

        /// <summary>
        /// Aligns the filestream to the previous multiple of alignment
        /// </summary>
        /// <param name="alignment">4, 8, 16 etc...</param>
        public void AlignBack(int alignment) =>
            fstream.Position = fstream.Position / alignment * alignment;
    }
}