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
    public class FileTranslator
    {
        /// <summary>
        /// Translates a SourceFile in Translationformat to a TextFile
        /// </summary>
        /// <param name="fileSource"></param>
        /// <param name="fileDest"></param>
        public static void TranslationFileToText(string fileSource, string fileDest)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileSource);

            string text = "";
            GetText(document.ChildNodes, ref text);

            File.WriteAllText(fileDest, text);
        }

        /// <summary>
        /// Gets text Text (Values) out of all Xml Nodes with Type "Seg"
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="text"></param>
        private static void GetText(XmlNodeList nodes, ref string text)
        {
            foreach (XmlNode child in nodes)
            {
                if (child.Name == "Seg")
                {
                    text += StripHTML(child.InnerText) + "\r\n";
                }

                GetText(child.ChildNodes, ref text);
            }
        }

        /// <summary>
        /// Remove Tags out of a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string StripHTML(string text)
        {
            return Regex.Replace(text, "<.*?>", String.Empty);
        }
    }
}
