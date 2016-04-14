using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace SeekAndArchive
{
    class Program
    {
        private static List<FileInfo> FoundFiles;
        private static List<FileSystemWatcher> watchers;
        private static List<DirectoryInfo> archiveDirs;


        static void ArchiveFile(DirectoryInfo archiveDir, FileInfo fileToArchive)
        {
            FileStream input = fileToArchive.OpenRead();
            FileStream output = File.Create(archiveDir.FullName + @"/" + fileToArchive + ".gz");

            GZipStream Compressor = new GZipStream(output, CompressionMode.Compress);

            int b = input.ReadByte();

            while (b != -1)
            {
                Compressor.WriteByte((byte)b);
                b = input.ReadByte();
            }

            Compressor.Close();
            input.Close();
            output.Close();
        }

        static void RecursiveSearch(List<FileInfo> foundFiles, string fileName, DirectoryInfo currentDirectory)
        {
            foreach (FileInfo fil in currentDirectory.GetFiles())
            {
                if (fil.Name == fileName)
                {
                    foundFiles.Add(fil);
                }
            }
            foreach (DirectoryInfo dir in currentDirectory.GetDirectories())
            {
                RecursiveSearch(foundFiles, fileName, dir);
            }
        }

        static void WatcherChanged(object sender, FileSystemEventArgs e)
        {
            //if (e.ChangeType==WatcherChangeTypes.Changed)
            //{
            //    Console.WriteLine($"{e.FullPath} has been changed!");
            //}
            Console.WriteLine($"{e.FullPath} has been changed!");

            FileSystemWatcher senderWatcher = (FileSystemWatcher) sender;
            int index = watchers.IndexOf(senderWatcher, 0);

            ArchiveFile(archiveDirs[index], FoundFiles[index]);
        }
        static void Main(string[] args)
        {
            string fileName = args[0];
            string directoryName = args[1];
            watchers = new List<FileSystemWatcher>();
            FoundFiles = new List<FileInfo>();


            DirectoryInfo rootDir = new DirectoryInfo(directoryName);
            if (!rootDir.Exists)
            {
                Console.WriteLine("The specified directory does not exist.");
                Console.ReadKey();
                return;
            }

            RecursiveSearch(FoundFiles,fileName,rootDir);
            Console.WriteLine($"Found {FoundFiles.Count} files.");
            foreach (FileInfo fil in FoundFiles)
            {
                Console.WriteLine($"{fil.FullName}");
            }
            
            foreach (FileInfo fil in FoundFiles)
            {
                FileSystemWatcher newWatcher = new FileSystemWatcher(fil.DirectoryName, fil.Name);
                newWatcher.Changed += new FileSystemEventHandler(WatcherChanged);

                newWatcher.EnableRaisingEvents = true;
                watchers.Add(newWatcher);
            }

            archiveDirs = new List<DirectoryInfo>();

            for (int i = 0; i < FoundFiles.Count; i++)
            {
                archiveDirs.Add(Directory.CreateDirectory("archive" + i.ToString()));
            }

            Console.ReadKey();


        }

    }
}
