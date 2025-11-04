namespace SproutVRSchool.Domain;

/// <summary>
/// Contains all constant name of the application
/// </summary>
public static class AppCts
{
    public static class Db
    {
        public const string AUTH_SCHEMA = "auth";
        public const string APP_SCHEMA = "app";

        public const string ROLE_TEACHER = "Teacher";
        public const string ROLE_SCHOOL_ADMIN = "School Admin";
    }

    public static class Accounts
    {
        // OTP
        public const int OTP_CODE_LENGTH = 6;
        public const int OTP_CODE_VALID_DURATION_IN_MINUTES = 5;

        // Password
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
        public const int DEFAULT_PAGE_SIZE = 5;
        public const bool DEFAULT_IS_PAGINATED = false;
    }

    public static class Redis
    {
        public const int MAX_VR_DEVICE = 30;
        public const int CODE_DURATION_IN_MINUTES = 2;
        public const int ACTIVE_VR_LEARNING_SESSIONS_SCAN_INTERVAL_IN_MILSECONDS = 5000;
        public const string NAMESPACE_VR_LEARNING_SESSIONS = "vr_learning_sessions";
        public const string NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE = "vr_learning_sessions_active";
        public const string NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS = "vr_learning_sessions_notify_events";
        public const string NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODE = "vr_learning_sessions_room_codes";
        public const string NAMESPACE_STREAM_EVENT_VR_LEARNING_SESSIONS = "stream:vr_learning_sessions";
    }

    public static class SeederFilePaths
    {
        // Get the folder at runtime
        private const string JSON_FOLDER_PATH = "Data/SeederFiles";

        // Each json file path
        public static readonly string MasterSubjectFilePath = Path.Combine(JSON_FOLDER_PATH, "MasterSubject.json");
        public static readonly string SubjectFilePath = Path.Combine(JSON_FOLDER_PATH, "Subject.json");
        public static readonly string ActivityTypeFilePath = Path.Combine(JSON_FOLDER_PATH, "ActivityType.json");
        public static readonly string MapFilePath = Path.Combine(JSON_FOLDER_PATH, "Map.json");
        public static readonly string MapObjectFilePath = Path.Combine(JSON_FOLDER_PATH, "MapObject.json");
        public static readonly string TaskLocationFilePath = Path.Combine(JSON_FOLDER_PATH, "TaskLocation.json");
        public static readonly string ObjectActivityTypeFilePath = Path.Combine(JSON_FOLDER_PATH, "ObjectActivityType.json");
        public static readonly string ObjectLocationFilePath = Path.Combine(JSON_FOLDER_PATH, "ObjectLocation.json");
        public static readonly string VRDeviceFilePath = Path.Combine(JSON_FOLDER_PATH, "VRDevice.json");
        public static readonly string LessonFilePath = Path.Combine(JSON_FOLDER_PATH, "Lesson.json");
        public static readonly string VRLessonFilePath = Path.Combine(JSON_FOLDER_PATH, "VRLesson.json");
    }

    public static class FilePaths
    {
        // NOT WORKING INSIDE THE CONTAINER

#pragma warning disable S125 // Sections of code should not be commented out
        // offical path to store files
        // macOS: /Library/Application Support
        // Windows: C:\ProgramData
        // Linux: /var/lib

        //// "C:\ProgramData\SproutVRSchool\Content\Avatars"
        //public static readonly string AvatarsFolderPath = Path.Combine(LocalContentRootPath, "Avatars");
        //public static readonly string DefaultAvatarFilePath = Path.Combine(AvatarsFolderPath, "default_avatar.png");
#pragma warning restore S125 // Sections of code should not be commented out

        public const string FOLDER_NAME_RESOURCES = "Resources";
        public const string FOLDER_NAME_PRESETS = "Presets";
        public const string FOLDER_NAME_IMAGES = "Images";
        public const string PREFIX_PUBLIC_CONTENT_PATH = "/content";
        public const string FILE_NAME_PRESET_VR_LESSON = "preset.json";
    }

    public static class Grpc
    {
        public const string ERROR_MESSAGE_KEY = "error_message";
    }

    public static class RetryKeys
    {
        public const string REDIS_TRANSACTION_KEY = "Redis_Transaction_Retry";
    }

    public static class SortingKeys
    {
        // Default sorting
        public const string DEFAULT = CREATED_AT_UTC_ASC;

        // Common Sorting fields
        public const string CREATED_AT_UTC_ASC = "createdAtUtcAsc";
        public const string CREATED_AT_UTC_DESC = "createdAtUtcDesc";
        public const string UPDATED_AT_UTC_ASC = "updatedAtUtcAsc";
        public const string UPDATED_AT_UTC_DESC = "updatedAtUtcDesc";

        public static class VRDevices
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class Maps
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class Subjects
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class MasterSubjects
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class Lessons
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class TaskLocations
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class ActivityTypes
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }

        public static class MapObjects
        {
            public const string NAME_ASC = "nameAsc";
            public const string NAME_DESC = "nameDesc";
        }
    }
}
