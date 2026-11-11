using System.IO;
using System.Text;

public class MultiWriter : TextWriter
{
    private readonly TextWriter[] _writers;

    public override Encoding Encoding => Encoding.UTF8;

    public MultiWriter(params TextWriter[] writers)
    {
        _writers = writers;
    }

    public override void Write(char value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    public override void Write(string value)
    {
        foreach (var writer in _writers)
            writer.Write(value);
    }

    public override void WriteLine(string value)
    {
        foreach (var writer in _writers)
            writer.WriteLine(value);
    }

    public override void Flush()
    {
        foreach (var writer in _writers)
            writer.Flush();
    }
}