namespace LawfulBladeManager.Type
{
    public static class PathExtensions
    {
        public static bool IsValid(string path)
        {
            if (!Path.Exists(path))
                return false;

            return true;
        }
    }
}
