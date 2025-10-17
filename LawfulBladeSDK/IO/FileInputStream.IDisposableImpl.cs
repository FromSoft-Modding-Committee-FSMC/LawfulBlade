namespace LawfulBladeSDK.IO
{
    public partial class FileInputStream
    {
        protected void Dispose(bool disposeManagedObjects)
        {
            if (disposeManagedObjects)
            {
                //Clear jumpstack because paranoid
                jumpStack.Clear();

                //Dispose internal stream
                fstream.Dispose();
                fstream = Stream.Null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        ~FileInputStream()
        {
            Dispose(false);
        }
    }
}