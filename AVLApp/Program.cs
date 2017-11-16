using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace AVLApp
{
    public class Program
    {
        private static string[,] FileMapping = new string[,]
        {
            { ".DEU", "_de.txt" },
            { ".ENU", "_en.txt" },
        };

        static void Main(string[] args)
        {
            string sourceDirectory = args[0];
            string targetDirectory = args[1];
            
            TranslateDirectory(sourceDirectory, targetDirectory);
            System.Console.WriteLine("Translation finished");
            System.Console.ReadLine();
        }   

        private static void TranslateDirectory(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            if(!diTarget.Exists)
            {
                diTarget.Create();
            }

            //Walk through all files inside a directory
            DirectoryInfo dirInfo = new DirectoryInfo(sourceDirectory);
            foreach (var file in dirInfo.GetFiles())
            {
                //Find out if it is a tranlation file
                if (IsTranslationFile(file.Extension.ToLower()))
                {
                    //and convert it
                    System.Console.WriteLine(string.Format("File {0} wird umgewandelt", file.FullName));
                    string targetFile = diTarget.FullName + "\\" + GetTargetFileName(file.Name, file.Extension);                    
                    FileTranslator.TranslationFileToText(file.FullName, targetFile);
                    System.Console.WriteLine(string.Format("File {0} geschrieben", targetFile));
                }
            }

            //Walk through the subdirectories
            foreach(var directory in dirInfo.GetDirectories())
            {
                TranslateDirectory(directory.FullName, diTarget.FullName + "\\" + directory.Name);
            }
        }


        /// <summary>
        /// Determines if the extension is a known Translationfileextension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static bool IsTranslationFile(string extension)
        {
            for(int i = 0; i < FileMapping.GetLength(0); i++)
            {
                if(FileMapping[i, 0].ToLower() == extension.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a targetFilename for TranslationFile 
        /// </summary>
        /// <param name="soureFileName"></param>
        /// <param name="sourceFileExtension"></param>
        /// <returns></returns>
        private static string GetTargetFileName(string soureFileName, string sourceFileExtension)
        {
            string targetFileName = soureFileName;

            for (int i = 0; i < FileMapping.GetLength(0); i++)
            {
                if (FileMapping[i, 0].ToLower() == sourceFileExtension.ToLower())
                {
                    return soureFileName.Replace(sourceFileExtension, "") + FileMapping[i, 1];
                }                
            }

            throw new Exception("Could not translate Filename");
        }
    }
}