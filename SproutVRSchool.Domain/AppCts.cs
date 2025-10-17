namespace SproutVRSchool.Domain;

public static class AppCts
{
    public static class Db
    {
        public const string AUTH_SCHEMA = "auth";
        public const string APP_SCHEMA = "app";

        public const string ROLE_TEACHER = "Teacher";
        public const string ROLE_SCHOOL_ADMIN = "School Admin";
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

    public static class Redis
    {
        public const int MAX_VR_DEVICE = 30;
        public const int CODE_DURATION_IN_MINUTES = 2;
        public const string NAMESPACE_VR_LEARNING_SESSION = "vr_learning_sessions";
        public const string NAMESPACE_ROOM_CODE = "room_codes";
        public const string NAMESPACE_ACTIVE_LEARNING_SESSIONS = "active_vr_learning_sessions";
        public const string NAMESPACE_STREAM_EVENT_VR_LEARNING_SESSION = "stream:vr_learning_session";
        public const int ACTIVE_VR_LEARNING_SESSIONS_SCAN_INTERVAL_IN_MILSECONDS = 5000;
    }

    public static class FilePaths
    {
        // offical path to store files
        // macOS: /Library/Application Support
        // Windows: C:\ProgramData
        // Linux: /var/lib
        private static readonly string CommonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static readonly string StorageRootPath = Path.Combine(CommonAppDataPath, "SproutVRSchool", "Content");

        // "C:\ProgramData\SproutVRSchool\Content\Avatars"
        public static readonly string AvatarsFolderPath = Path.Combine(StorageRootPath, "Avatars");
        public static readonly string DefaultAvatarFilePath = Path.Combine(AvatarsFolderPath, "default_avatar.png");

        public const string FOLDER_NAME_RESOURCES = "Resources";
        public const string FOLDER_NAME_PRESETS = "Presets";
        public const string FOLDER_NAME_IMAGES = "Images";
        public const string PREFIX_PUBLIC_CONTENT_PATH = "/content";
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
        public static readonly string VRDeviceFilePath = Path.Combine(JsonFolderPath, "VRDevice.json");
        public static readonly string LessonFilePath = Path.Combine(JsonFolderPath, "Lesson.json");
        public static readonly string VRLessonFilePath = Path.Combine(JsonFolderPath, "VRLesson.json");
    }
}
