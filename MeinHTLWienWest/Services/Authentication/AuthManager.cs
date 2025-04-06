using CryptoLibrary.Encryption;
using Dapper;
using MailKit.Net.Smtp;
using MimeKit;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;

namespace MeinHTLWienWest.Services.Authentication
{
    public static class AuthManager
    {
        public static byte[] TokenKey { get; } = [ 184, 255, 59, 250, 106, 110, 63, 203, 211, 162, 249, 129, 28, 131, 110, 50, 203, 162, 82, 95, 88, 20, 107, 120, 16, 161, 107, 160, 114, 23, 246, 134 ];
        public static byte[] TokenIv { get; } = [107, 32, 92, 217, 156, 182, 176, 86, 216, 125, 143, 228, 67, 16, 136, 199 ];

        public static bool SendOTPCode(string otpCode, string email)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Mein HTL Wien West", "malbasicmarcel@cerasis.eu"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Anmeldeversuch";
                message.Body = new TextPart("plain")
                {
                    Text = $"Vielen Dank für die Benutzung von Mein HTL Wien West! \n" +
                            $"Der Code für die Authentifizierung lautet:\n\n{otpCode}"
                };

                Task.Run(async () =>
                {
                    using (var client = new SmtpClient())
                    {
                        try
                        {
                            client.Connect("www.cerasis.eu", 587, false);
                            client.Authenticate("malbasicmarcel@cerasis.eu", "!.S0m3th1ng?33");
                            client.Send(message);
                            client.Disconnect(true);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static async Task<string> Login()
        {
            string status = "";
            var dbObject = new { Email = Helper.CurrentEmail, LastLogin = DateTime.Now };
            //register
            var conn = Helper.GetConnection();
            await conn.ExecuteAsync(Helper.SQLQueries["refresh_last_login"].Statement, dbObject);
            Helper.CurrentUserId = (await conn.QueryAsync<int>(Helper.SQLQueries["get_user_id"].Statement, new { Email = Helper.CurrentEmail })).FirstOrDefault();

            return "";
        }

        public static async Task<string> Register(string selectedRole)
        {
            string status = "";
            var dbObject = new { Email = Helper.CurrentEmail, LastLogin = DateTime.Now };
            //register
            var conn = Helper.GetConnection();
            await conn.ExecuteAsync(Helper.SQLQueries["add_new_user"].Statement, dbObject);
            Helper.CurrentUserId = (await conn.QueryAsync<int>(Helper.SQLQueries["get_user_id"].Statement, new { Email = Helper.CurrentEmail })).FirstOrDefault();

            //set privileges according to dropdown select

            List<int> privileges = new List<int>();

            switch (selectedRole)
            {
                case "Schüler":
                    privileges = new List<int>() { 2, 3, 4 };
                    break;
                case "Lehrer":
                    privileges = new List<int>() { 1, 2, 3, 4 };
                    break;
                default:
                    status = "Bitte Ihre Position eintragen";
                    return status;


            }

            for (int i = 0; i < privileges.Count; i++)
            {
                await conn.ExecuteAsync(Helper.SQLQueries["add_privilege"].Statement, new { UserId = Helper.CurrentUserId, PrivilegeId = privileges[i] });
            }


            return "";

        }

        public static async Task LoadUserDetails()
        {
            var conn = Helper.GetConnection();
            if (Helper.CurrentUserId == 0)
            {
                Helper.CurrentUserId = (await conn.QueryAsync<int>(Helper.SQLQueries["get_user_id"].Statement, new { Email = Helper.CurrentEmail })).FirstOrDefault();
            }

            Helper.UserPermissions = (await conn.QueryAsync<string>(Helper.SQLQueries["get_user_permissions"].Statement, new { UserId = Helper.CurrentUserId })).ToList();

        }


        public static async Task<string> GetCerasisToken(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Email", email);
                client.DefaultRequestHeaders.Add("Password", password);
                using HttpResponseMessage response = await client.GetAsync("https://www.cerasis.eu/api/remote-auth");
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        AES tokenEncryption = new AES();
                        string receivedToken = tokenEncryption.DecryptString(Convert.FromBase64String(responseBody), AuthManager.TokenKey, AuthManager.TokenIv);
                        Helper.CerasisToken = responseBody;
                        
                        return receivedToken;
                    }
                    catch (Exception)
                    {
                        return "Error";
                    }
                   
                }
              
            }
        }

        
        //public static async Task GiveCerasis()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var requestBody = new
        //        {
        //            Card = Helper.CerasisCardId,
        //            Amount = 30
        //        };

        //        HttpResponseMessage response = await client.PostAsJsonAsync("https://www.cerasis.eu/", requestBody);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseData = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine("Response: " + responseData);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Error: {response.StatusCode}");
        //        }
        //    }
        //}
    }
}
