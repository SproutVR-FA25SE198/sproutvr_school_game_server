using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Domain;

public static class AppCts
{
    public static class Db
    {
        public const string AUTH_SCHEMA = "auth";
        public const string APP_SCHEMA = "app";
    }

    public static class TimeOffSet
    {
        public const int VN = +7;
    }

    public static class Api
    {
        public const string V1 = "1";
        public const string V1_1 = "1.1";
        public const string V2 = "2";

        public const int DEFAULT_PAGE_INDEX = 1;
        public const int DEFAULT_PAGE_SIZE = 10;
        public const int DEFAULT_TOTAL_PAGES = 1;
        public const int DEFAULT_COUNT = 0;
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
