using System.Diagnostics;
using System.Text.RegularExpressions;
using NukeX;

var sw = Stopwatch.StartNew();

Console.WriteLine("*** NukeX ***");

var rootFolder = NukeXHelper.FindRootDirectory(".nuke");
if (rootFolder != null)
{
    Console.WriteLine("Nuke root folder: " + rootFolder);
}
else
{
    Console.WriteLine("Cannot find root .nuke folder");
    Environment.Exit(1);
    return;
}

var ps1FilePath = Path.Combine(rootFolder, "build.ps1");
if (File.Exists(ps1FilePath))
{
    Console.WriteLine("Nuke build.ps1 file: " + ps1FilePath);
}
else
{
    Console.WriteLine("Cannot find build.ps1 file");
    Environment.Exit(1);
    return;
}

// read using regex from file ps1FilePath and find path. Line in file like this: $BuildProjectFile = "$PSScriptRoot\build\_build.csproj"
var regex = new Regex(@"\$BuildProjectFile = ""\$PSScriptRoot\\(?<path>.*)""");
var match = regex.Match(File.ReadAllText(ps1FilePath));
string buildProjectFilePath = "";

if (match.Success)
{
    buildProjectFilePath = Path.Combine(rootFolder, match.Groups["path"].Value);
    Console.WriteLine("Nuke build project file: " + buildProjectFilePath);
}
else
{
    Console.WriteLine("Cannot find build project file");
    Environment.Exit(1);
    return;
}

var buildProjectDirectory = Path.GetDirectoryName(buildProjectFilePath);
var buildProjectDirectoryOutput = Path.Combine(buildProjectDirectory, "bin", "NukeX");

var fileList = NukeXHelper.GetFiles(buildProjectDirectory,
    new[] { "bin" + Path.DirectorySeparatorChar, "obj" + Path.DirectorySeparatorChar }).ToList();
var hash = NukeXHelper.CalculateHash(fileList);
var buildProjectHashFilePath = Path.Combine(buildProjectDirectoryOutput, "build.hash");
if (!File.Exists(buildProjectHashFilePath) || File.ReadAllText(buildProjectHashFilePath) != hash)
{
    if (File.Exists(buildProjectHashFilePath))
    {
        File.Delete(buildProjectHashFilePath);
    }


    Process.Start(new ProcessStartInfo("dotnet",
            $"build \"{buildProjectFilePath}\" --configuration Release --output \"{buildProjectDirectoryOutput}\"")
        {
            UseShellExecute = false,
        })
        ?.WaitForExit();

    File.WriteAllText(buildProjectHashFilePath, hash);
}

var buildProjectExeFile = NukeXHelper.FindFiles(buildProjectDirectoryOutput, "*.exe").FirstOrDefault();
if (buildProjectExeFile != null)
{
    Console.WriteLine("Nuke build project exe file: " + buildProjectExeFile);
}
else
{
    Console.WriteLine("Cannot find build project exe file");
    Environment.Exit(1);
    return;
}


// execute buildProjectExeFile executable file with complete same arguments as this program (dot not forget escaping)
var arguments = string.Join(" ", args.Select(x => $"\"{x}\""));


sw.Stop();
Console.WriteLine($"NukeX build time: {sw.ElapsedMilliseconds} ms");
Process.Start(new ProcessStartInfo(buildProjectExeFile, arguments)
    {
        UseShellExecute = false,
    })
    ?.WaitForExit();