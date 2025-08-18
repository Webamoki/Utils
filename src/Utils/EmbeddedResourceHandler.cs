using System.Reflection;

namespace Webamoki.Utils;

/// <summary>
/// Provides helper methods to access embedded resources in a specified assembly. The assembly must be registered in the Webamoki-environment.
/// </summary>
public static class EmbeddedResourceHandler
{

    /// <summary>
    /// To access an embedded resource, ensure the provided <paramref name="path"/> is in the format Test.Image.png instead of Test/Image.png
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Stream? Read(Assembly assembly, string path)
    {
        var assemblyPrefix = assembly.GetName().Name + ".Files.";
        return assembly.GetManifestResourceStream($"{assemblyPrefix}{path}");
    }

    /// <summary>
    /// To check if an embedded resource exists, ensure the provided <paramref name="path"/> is in the format Test.Image.png instead of Test/Image.png
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="path"></param>
    /// <returns>If any resource in the Assembly matches the provided path</returns>
    public static bool Exists(Assembly assembly, string path)
    {
        var assemblyPrefix = assembly.GetName().Name + ".Files.";
        var resources = assembly.GetManifestResourceNames();
        return resources.Any(r => r.Contains($"{assemblyPrefix}{path}"));
    }
}
