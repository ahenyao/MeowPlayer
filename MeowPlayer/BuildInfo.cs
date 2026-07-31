using System.Linq;
using System.Reflection;
using Git = GitInfo.ThisAssembly.Git;

namespace MeowPlayer;

public static class BuildInfo {

    static string GetKeyFromAssembly(string key) {
        string value = Assembly.GetExecutingAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(m => m.Key == key)?.Value ?? "<unknown>";
        return value;
    }
    
    
    public static string CompileDate {
        get {
            if (string.IsNullOrEmpty(field))
                field = GetKeyFromAssembly("BuildDate");
            return field;
        }
    }
    
    public static string BuildHostOS {
        get {
            if (string.IsNullOrEmpty(field))
                field = GetKeyFromAssembly("BuildHostOS");
            return field;
        }
    }
    
    public static string BuildHostArch {
        get {
            if (string.IsNullOrEmpty(field))
                field = GetKeyFromAssembly("BuildHostArch");
            return field;
        }
    }

    public static string Version {
        get {
            if (string.IsNullOrEmpty(field)) {
                field = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "<unknown>";
            }
            return field;
        }
    }

    public static string GitCommit {
        get {
            if (string.IsNullOrEmpty(field)) {
                bool isDirty = Git.IsDirty;
                field = $"{Git.Commit}{(isDirty ? "-dirty" : "")}";
            }
            return field;
        }
    }
    
}