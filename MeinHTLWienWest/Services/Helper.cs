using Microsoft.JSInterop;
using MimeKit.Tnef;
using Npgsql;
using System.ComponentModel;

namespace MeinHTLWienWest.Services
{
    public static class Helper
    {
        public static bool IsSent { get; set; }
        public static string CurrentEmail { get; set; }
        public static int CurrentUserId { get; set; } = -1;
        public static bool SetMapPoint { get; set; }
        public static Marker ProblemMarker { get; set; } = new Marker() { X = -1, Y = -1 };
        public static int SelectedMarkerFloor { get; set; } = 0;
        public static SubmitTicketDTO TicketToSubmit { get; set; } = new SubmitTicketDTO();
        public static string CerasisToken { get; set; } 

        public static bool IsNewUser { get; set; }

        public static string DBConnString { private get; set; }

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new(DBConnString);
            return connection;
        }

        public static List<string> UserPermissions { get; set; } = new List<string>();
        public static Dictionary<string, SQLQuery> SQLQueries { get; set; }


        //Cerasis

        public static string CerasisCardId { get; set; }
    }

    public class JsConsole
    {
        private readonly IJSRuntime JsRuntime;
        public JsConsole(IJSRuntime jSRuntime)
        {
            this.JsRuntime = jSRuntime;
        }

        public async Task LogAsync(string message)
        {
            await this.JsRuntime.InvokeVoidAsync("console.log", message);
        }
    }
}
