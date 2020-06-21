using System.Collections.Generic;

namespace DataRoom.Models
{
    public class FileFolderModel
    {
        public string FolderPath { get; set; }
        public List<FileObjectModel> FileObjects { get; set; } = new List<FileObjectModel>();
    }
}
