using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MusicSpot.Models
{
    public static class DirectoryManager
    {
        private static readonly string _musicDirectoriesFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MusicSpot\MusicDirectories";

        public static void InitMusicFilesDirectories()
        {
            var doc = new XDocument();
            var directoriesElement = new XElement("directories");
            directoriesElement.Add(new XElement("directory", new XElement("path", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))));
            doc.Add(directoriesElement);
            doc.Save(_musicDirectoriesFilePath);
        }

        public static IList<string> LoadMusicDirectoriesFromXML()
        {
            if (!File.Exists(_musicDirectoriesFilePath))
            {
                InitMusicFilesDirectories();
            }

            IList<string> directoriesPathsList = new List<string>();

            var document = XDocument.Load(_musicDirectoriesFilePath);
            var directories = document.XPathSelectElements("/directories/directory");
            foreach (var directory in directories)
            {
                var directoryPath = directory.Descendants("path").FirstOrDefault()?.Value;
                if (!string.IsNullOrWhiteSpace(directoryPath))
                {
                    directoriesPathsList.Add(directoryPath);
                }
            }

            return directoriesPathsList;
        }

        public static void SaveMusicDirectoryToXML(string directoryToAdd)
        {
            var document = XDocument.Load(_musicDirectoriesFilePath);
            var newDirectoryElement = new XElement("directory", new XElement("path", directoryToAdd));
            var directoriesElement = document.Element("directories");
            directoriesElement?.Add(newDirectoryElement);
            document.Save(_musicDirectoriesFilePath);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static void RemoveMusicDirectoryFromXML(string directoryToRemove)
        {
            var document = XDocument.Load(_musicDirectoriesFilePath);
            var directories = document.XPathSelectElements("/directories/directory");
            foreach (var directory in directories)
            {
                if (!string.IsNullOrWhiteSpace(directory.Descendants("path").FirstOrDefault()?.Value) &&
                    directory.Descendants("path").FirstOrDefault().Value.Equals(directoryToRemove))
                {
                    directory.Remove();
                }
            }

            document.Save(_musicDirectoriesFilePath);
        }
    }
}