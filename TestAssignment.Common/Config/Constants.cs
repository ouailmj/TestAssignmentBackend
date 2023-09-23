namespace TestAssignment.Common.Config
{
    /// <summary>
    /// Contains common global constants used in app
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Regular expression pattern for parsing movie data.
        /// </summary>
        public static readonly String DATA_MOVIES_PATTERN = @"(\d+)::(.+?) \((\d+)\)::(.+)";

        /// <summary>
        /// Prefix used for generating dummy usernames.
        /// </summary>
        public static readonly String DUMMY_USERNAME_PREFIX = "user";

        /// <summary>
        /// Suffix used for generating dummy email addresses.
        /// </summary>
        public static readonly String DUMMY_EMAIL_SUFFIX = "@example.com";

        /// <summary>
        /// Default password for dummy users.
        /// </summary>
        public static readonly String DUMMY_USER_PASSWORD = "P@ssw0rd123";

        /// <summary>
        /// List of dummy movie names used for seeding data.
        /// </summary>
        public static readonly List<String> DUMMY_MOVIES_NAMES = new List<string> { 
            "Toy Story", "Jumanji", "Grumpier Old Men", "Waiting to Exhale", "Father of the Bride Part II", "Heat", "Sabrina", "Tom and Huck", 
            "Sudden Death", "GoldenEye", "American President", "Dracula: Dead and Loving It", "Balto", "Nixon", "Cutthroat Island", "Casino", 
            "Sense and Sensibility", "Four Rooms", "Ace Ventura: When Nature Calls", "Money Train"
        };

        /// <summary>
        /// number of max elasticsearch records used when getting datas.
        /// </summary>
        public static readonly int MAX_NUMBER_ELASTICSEARCH_RECORDS = 20;

    }
}