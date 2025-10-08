using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Domain;

public static class AppCts
{
    public static class DB
    {
        public const string AUTH_SCHEMA = "auth";
        public const string APP_SCHEMA = "app";
    }

    public static class SeederFilePaths
    {
        // Get the folder at runtime
        private const string JsonFolderPath = "Data/SeederFiles";

        // Each json file path
        public static readonly string MasterSubjectFilePath = Path.Combine(JsonFolderPath, "MasterSubject.json");
        public static readonly string SubjectFilePath = Path.Combine(JsonFolderPath, "Subject.json");
        public static readonly string ActivityTypeFilePath = Path.Combine(JsonFolderPath, "ActivityType.json");
        public static readonly string MapFilePath = Path.Combine(JsonFolderPath, "Map.json");
        public static readonly string MapObjectFilePath = Path.Combine(JsonFolderPath, "MabObject.json");
        public static readonly string TaskLocationFilePath = Path.Combine(JsonFolderPath, "TaskLocation.json");
        public static readonly string ObjectActivityTypeFilePath = Path.Combine(JsonFolderPath, "ObjectActivityType.json");
        public static readonly string ObjectLocationFilePath = Path.Combine(JsonFolderPath, "ObjectLocation.json");
    }
}
