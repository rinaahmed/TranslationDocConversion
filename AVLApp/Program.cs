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
        private const string DefaultTargetFileExtension = "ALIGN";

        // Contains Mapping Table that is read from configuration
        private static Dictionary<string, string> FileMapping = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            if (!ValidateArguments(args))
            {
                return;
            }

            string sourceDirectory = args[0];
            string targetDirectory = args[1];
            string targetFileExtension = args.Length > 2 ? args[2] : DefaultTargetFileExtension;
            ReadFileMappingFromConfig();
            
            TranslateDirectory(sourceDirectory, targetDirectory, targetFileExtension);
            System.Console.WriteLine("Translation finished");
            System.Console.ReadLine();
        }

        /// <summary>
        /// Reads FileMap Configuration Data into Dictionary FileMapping
        /// </summary>
        private static void ReadFileMappingFromConfig()
        {
            int i = 1;
            string fileMap = System.Configuration.ConfigurationManager.AppSettings["FileMap" + i.ToString()];
            while(!String.IsNullOrEmpty(fileMap))
            {
                var fileMapParts = fileMap.Split(';');
                FileMapping.Add(fileMapParts[0], fileMapParts[1]);
                i++;
                fileMap = System.Configuration.ConfigurationManager.AppSettings["FileMap" + i.ToString()];
            }            
        }

        /// <summary>
        /// Validates Commandline Arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns>false if not valid</returns>
        private static bool ValidateArguments(string[] args)
        {
            bool isValid = true;

            if(args.Length < 2)
            {
                System.Console.WriteLine("Es müssen zumindest 2 Parameter übergeben werden.");
                isValid = false;
            }

            if(isValid && !Directory.Exists(args[0]))
            {
                System.Console.WriteLine(string.Format("Der erste Parameter {0} ist kein gültiger Vereichnisname", args[0]));
                isValid = false;
            }

            if (isValid && !Directory.Exists(args[1]))
            {
                System.Console.WriteLine(string.Format("Der zweite Parameter {0} ist kein gültiger Vereichnisname", args[1]));
                isValid = false;
            }

            if(!isValid)
            {
                System.Console.WriteLine("\n\rLeider wurden die Paramter nicht korrekt übergeben:\n\r" +
                                         "Parameter1: [InputDirectory]  - verpflichtend\n\r" +
                                         "Parameter2: [OutputDirectory] - verpflichtend\n\r" +
                                         "Parameter3: [TargetExtension] - optional\n\r" +
                                         "Beispiel: AVLApp.exe \"c:\\input\" \"c:\\output\" txt\n\r");

            }

            return isValid;
        }

        private static void TranslateDirectory(string sourceDirectory, string targetDirectory, string targetFileExtension)
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
                    string targetFile = diTarget.FullName + "\\" + GetTargetFileName(file.Name, file.Extension, targetFileExtension);                    
                    FileTranslator.TranslationFileToText(file.FullName, targetFile);
                    System.Console.WriteLine(string.Format("File {0} geschrieben", targetFile));
                }
            }

            //Walk through the subdirectories
            foreach(var directory in dirInfo.GetDirectories())
            {
                TranslateDirectory(directory.FullName, diTarget.FullName + "\\" + directory.Name, targetFileExtension);
            }
        }


        /// <summary>
        /// Determines if the extension is a known Translationfileextension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static bool IsTranslationFile(string extension)
        {
            return FileMapping.Keys.ToList().Exists(k => k.ToLower() == extension);            
        }

        /// <summary>
        /// Creates a targetFilename for TranslationFile 
        /// </summary>
        /// <param name="soureFileName"></param>
        /// <param name="sourceFileExtension"></param>
        /// <returns></returns>
        private static string GetTargetFileName(string soureFileName, string sourceFileExtension, string targetFileExtension)
        {
            string targetFileName = soureFileName;

            string key = FileMapping.Keys.ToList().FirstOrDefault(k => k.ToLower() == sourceFileExtension.ToLower());
            if(!string.IsNullOrEmpty(key))
            {
                return soureFileName.Replace(sourceFileExtension, "") + FileMapping[key] + "." + targetFileExtension;
            }          

            throw new Exception("Could not translate Filename");
        }
    }
}