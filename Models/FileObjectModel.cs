using System;

namespace DataRoom.Models
{
    public class FileObjectModel
    {
        public string Type { get; set; }
        public string Parent { get; set; }
        public string ItemPath { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public static string TopLevelDataFolder { get; set; }
        public static string InternalDataPath { get; set; }
    }
}
