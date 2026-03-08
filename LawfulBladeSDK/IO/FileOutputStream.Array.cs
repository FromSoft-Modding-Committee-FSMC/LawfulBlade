namespace LawfulBladeSDK.IO
{
    public partial class FileOutputStream
    {
        public void WriteU8Array(byte[] u8s) =>
            fstream.Write(u8s, 0, u8s.Length);
    }
}
