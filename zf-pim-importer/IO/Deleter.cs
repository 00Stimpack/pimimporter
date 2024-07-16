using System;
using System.IO;
//using System.Threading;

namespace ZFPimImporter.IO
{
    public class Deleter
    {
        

        public static void DeleteFolder(string folderPath)
        {
        

            try
            {
                DeleteFolderSafely(folderPath);

                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("Folder deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to delete the folder.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void DeleteFolderSafely(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                // Remove readonly attributes from files and folders
                RemoveReadOnlyAttributes(folderPath);

                // Delete folder recursively
                Directory.Delete(folderPath, true);
            }
            else
            {
                throw new DirectoryNotFoundException("The folder does not exist.");
            }
        }

        public  static void RemoveReadOnlyAttributes(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            // Remove readonly attribute from the folder
            if (directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
            }

            // Remove readonly attributes from all files in the folder
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Attributes.HasFlag(FileAttributes.ReadOnly))
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }
            }

            // Recursively remove readonly attributes from subfolders
            foreach (DirectoryInfo subFolder in directoryInfo.GetDirectories())
            {
                RemoveReadOnlyAttributes(subFolder.FullName);
            }
        }
    }
}