namespace metricas.utils;

public class FilesManager
{
    
    public static bool IsValidPath(string path)
    {
        return Directory.Exists(path) || (File.Exists(path) && path.EndsWith(".html"));
    }
    
    public static bool IsFolder(string path)
    {
        return Directory.Exists(path);
    }
    
    public static List<string> GetAllHtmlFilesFromDirectory(string path)
    {
        return Directory.GetFiles(path, "*.html", SearchOption.AllDirectories).ToList();
    }

}