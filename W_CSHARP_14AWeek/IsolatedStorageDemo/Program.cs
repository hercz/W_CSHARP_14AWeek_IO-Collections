﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsolatedStorageDemo
{
    class Program
    {
        static void Main(string[] args)
        {



            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.set", FileMode.Create, userStore);

            StreamWriter userWriter = new StreamWriter(userStream);
            userWriter.WriteLine("User Prefs, and what u want...");
            userWriter.Close();

            string[] files = userStore.GetFileNames("UserSettings.set");

            if (files.Length == 0)
            {
                Console.WriteLine("The file not exist!");
            }

            userStream = new IsolatedStorageFileStream("UserSettings.set", FileMode.Open, userStore);
            StreamReader userReader = new StreamReader(userStream);
            string contents = userReader.ReadToEnd();
            Console.WriteLine(contents);
            Console.ReadKey();
        }
    }
}
