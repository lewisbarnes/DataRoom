using System.IO;
using System.Linq;
using DataRoom.Models;
using Microsoft.Extensions.Configuration;

namespace DataRoom.Services
{
    public class FileDataService
    {
        private readonly IConfiguration configuration;
        private readonly string internalDataPath;
        public string TopLevelDataFolder { get; private set; }

        public FileDataService(IConfiguration config)
        {
            configuration = config;
            internalDataPath = configuration.GetValue<string>("InternalDataPath");
            TopLevelDataFolder = configuration.GetValue<string>("TopLevelDataFolder");
        }

        public FileFolderModel GetDataForPath(string path)
        {
            var ret = new FileFolderModel();
            

            if (string.IsNullOrEmpty(path))
            {
                path = TopLevelDataFolder;
            }
            string completePath = GetFullPath(path);
            if (Directory.Exists(completePath))
            {
                foreach (FileSystemInfo fsi in new DirectoryInfo(completePath).GetFileSystemInfos())
                {
                    ret.FileObjects.Add(new FileObjectModel()
                    {
                        DisplayName = fsi.Name,
                        ItemPath = IsPathTopLevel(path) ? fsi.Name : Path.Combine(path, fsi.Name),
                        Parent = path,
                        Type = fsi is FileInfo ? "Document" : "Folder",
                        LastWriteTime = fsi.LastWriteTime
                    });
                }
            }
            ret.FileObjects = ret.FileObjects.OrderBy(x => x.DisplayName).ToList();
            ret.FolderPath = path;
            return ret;
        }

        public string CreateNewFolder(string path, string folderName)
        {
            string newPath = Path.Combine(GetFullPath(path), folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            return Path.Combine(path,folderName);
        }

        public bool DeleteFileOrFolderForPath(string path)
        {
            if(IsPathFile(path))
            {
                File.Delete(GetFullPath(path));
                return true;
            } else if(IsPathFolder(path))
            {
                Directory.Delete(GetFullPath(path));
                return true;
            }
            return false;
        }

        public byte[] GetDataForFile(string filePath)
        {
            if (File.Exists(GetFullPath(filePath)))
            {
                return File.ReadAllBytes(GetFullPath(filePath));
            }
            return new byte[] { };
        }

        public bool PathExists(string path)
        {
            return Directory.Exists(GetFullPath(path)) || File.Exists(GetFullPath(path));
        }

        public string GetFullPath(string path)
        {
            if (IsPathTopLevel(path))
            {
                return Path.Combine(internalDataPath, path);
            }
            else
            {
                return Path.Combine(internalDataPath, TopLevelDataFolder, path);
            }
        }

        public bool IsPathTopLevel(string path)
        {
            return path == TopLevelDataFolder;
        }

        public bool IsPathFile(string path)
        {
            FileAttributes fileAttributes = System.IO.File.GetAttributes(GetFullPath(path));
            return (fileAttributes & FileAttributes.Directory) != FileAttributes.Directory;
        }

        public bool IsPathFolder(string path)
        {
            FileAttributes fileAttributes = System.IO.File.GetAttributes(GetFullPath(path));
            return (fileAttributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public string GetSubjectFolderName(string path)
        {
            return path.Replace($"{TopLevelDataFolder}\\", "").Split('\\')[0];
        }

        public string GetParentPathForFileOrFolder(string path)
        {
            string ret = string.Empty;
            if(IsPathFolder(path))
            {
                ret = Directory.GetParent(path).FullName;
            } else if(IsPathFile(path))
            {
                FileInfo fi = new FileInfo(path);
                ret = fi.Directory.Name;
            }

            return ret;
        }

        public string GetRelativeSubjectPath(string path)
        {
            return path.Replace(TopLevelDataFolder, "").Replace(internalDataPath,"");
        }
    }
}
