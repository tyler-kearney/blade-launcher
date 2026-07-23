using System;
using System.Collections.Generic;
using System.IO;
using blade_launcher.Models;

namespace blade_launcher.Core;

public class MacAppIndexer
{
    private static readonly string[] SearchDirectories =
    {
        "/Applications",
        "/System/Applications",
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Applications")
    };

    public List<AppEntry> ScanApplications()
    {
        var apps = new List<AppEntry>();

        foreach (var dir in SearchDirectories)
        {
            if (!Directory.Exists(dir)) continue;

            try
            {
                // Find top-level .app bundles in search directories
                var appDirectories = Directory.GetDirectories(dir, "*.app", SearchOption.TopDirectoryOnly);

                foreach (var appPath in appDirectories)
                {
                    string name = Path.GetFileNameWithoutExtension(appPath);
                    apps.Add(new AppEntry(name, appPath, string.Empty));
                }
            }
            catch (Exception e)
            {
                // Suppress directory permission issues
                Console.WriteLine(e);
                throw;
            }
        }

        return apps;
    }
}