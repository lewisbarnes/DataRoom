using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataRoom.Models
{
    public class FileSystemModel
    {
        public string Type { get; set; }
        public string Parent { get; set; }
        public string ItemPath { get; set; }
        public string DisplayName { get; set; }

        public static List<FileSystemModel> GetListDataForPath(string path)
        {
            var ret = new List<FileSystemModel>();
            var dirInfo = new DirectoryInfo(path);
            var fsinfos = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fsi in fsinfos)
            {
                if (fsi is FileInfo)
                {
                    ret.Add(new FileSystemModel() { DisplayName = fsi.Name, ItemPath = Path.Combine(path, fsi.Name), Parent = path, Type = "File" });
                }
                else if (fsi is DirectoryInfo)
                {
                    ret.Add(new FileSystemModel() { DisplayName = fsi.Name, ItemPath = Path.Combine(path, fsi.Name), Parent = path, Type = "Directory" });
                }
            }
            ret = ret.OrderBy(x => x.DisplayName).ToList();
            return ret;
        }
    }
}
