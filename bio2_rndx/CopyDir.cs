using System;
using System.IO;





    public static class CopyDir
    {
       

        public static void Copy(string sourceDirectory, string targetDirectory, byte dbg_flag)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget, dbg_flag);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target, byte debug_flag)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                if (debug_flag == 1)
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                }
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir, debug_flag);
            }
        }

        //public static void Main()
        //{
        //    string sourceDirectory = @"c:\sourceDirectory";
        //    string targetDirectory = @"c:\targetDirectory";

        //    Copy(sourceDirectory, targetDirectory);
        //}

        // Output will vary based on the contents of the source directory.
    }
