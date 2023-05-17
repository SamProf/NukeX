using System.Security.Cryptography;

namespace NukeX;

public static class NukeXHelper
{
    // find from current directory and upper folder which contains special folder name 
    public static string? FindRootDirectory(string specialFolderName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var rootDirectory = currentDirectory;
        while (true)
        {
            if (Directory.Exists(Path.Combine(rootDirectory, specialFolderName)))
            {
                return rootDirectory;
            }

            var parentDirectory = Directory.GetParent(rootDirectory);
            if (parentDirectory == null)
            {
                return null;
            }

            rootDirectory = parentDirectory.FullName;
        }
    }

    // function to find files by mask in specified folder
    public static IEnumerable<string> FindFiles(string folder, string mask)
    {
        var files = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly);
        return files;
    }


    public static IEnumerable<string> GetFiles(string path, string[] excludes)
    {
        path = Path.GetFullPath(path);
        foreach (var file in new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories))
        {
            var isExclude = false;
            foreach (var exclude in excludes)
            {
                if (file.FullName.StartsWith(Path.Combine(path, exclude), StringComparison.InvariantCultureIgnoreCase))
                {
                    isExclude = true;
                    continue;
                }
            }

            if (isExclude)
            {
                continue;
            }
            yield return file.FullName;
        }
    }
    
    
    // function to recieve list of file and calculate hash for all tougether
    public static string CalculateHash(IEnumerable<string> files)
    {
        var hash = SHA256.Create();
        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }

        hash.TransformFinalBlock(new byte[0], 0, 0);
        return BitConverter.ToString(hash.Hash).Replace("-", "").ToLowerInvariant();
    }
    
}