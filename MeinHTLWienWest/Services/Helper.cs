using Npgsql;
using System.ComponentModel;

namespace MeinHTLWienWest.Services
{
    public static class Helper
    {
        public static bool IsSent { get; set; }
        public static string CurrentEmail { get; set; }
        public static int CurrentUserId { get; set; }
        public static bool IsNewUser { get; set; }

        public static string DBConnString { private get; set; }

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new(DBConnString);
            return connection;
        }

        public static List<string> UserPermissions { get; set; }
        public static Dictionary<string, SQLQuery> SQLQueries { get; set; }


        //Cerasis

        public static string CerasisCardId { get; set; }
    }
}
