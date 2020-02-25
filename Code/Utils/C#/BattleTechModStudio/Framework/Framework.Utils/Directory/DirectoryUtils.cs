using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework.Utils.Directory
{
    public static class DirectoryUtils
    {
        public static string ConstructDateSegmentedDirectoryPath(string baseDirectory)
        {
            var dateSegmentedRelativeDirectory = $"{DateTime.Today.Year}\\{DateTime.Today.Month}\\{DateTime.Today.Day}";
            var finalDirectory = Path.Combine(baseDirectory, dateSegmentedRelativeDirectory);

            return finalDirectory;
        }

        public static void DirectoryCopy(string source, string dest, bool copySubDirs, List<string> filesToCopy)
        {
            DirectoryUtils.EnsureExists(dest);
            var di = new DirectoryInfo(source);

            di.GetFiles().ToList().ForEach(
                fi =>
                {
                    if (filesToCopy == null || filesToCopy.Contains(fi.Name))
                    {
                        File.Copy(fi.FullName, Path.Combine(dest, fi.Name), true);
                    }
                });

            if (copySubDirs)
            {
                di.GetDirectories().ToList().ForEach(
                    subDi => DirectoryUtils.DirectoryCopy(subDi.FullName, Path.Combine(dest, subDi.Name), true, filesToCopy));
            }
        }

        public static void EnsureExists(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
        }

        public static bool Exists(string directory)
        {
            return System.IO.Directory.Exists(directory);
        }
    }
}