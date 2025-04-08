using CryptoLibrary.Encryption;
using MeinHTLWienWest.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;

namespace MeinHTLWienWest.Services
{
    public class CerasisManager
    {
        [Inject]
        private JsConsole Console { get; set; }

        static List<TransactionDTO> transactionBuffer = new List<TransactionDTO>();
        public static async Task<string> GiveCerasis(TransactionReason reason)
        {
            double amount = 0.0;
           
            switch (reason)
            {
                case TransactionReason.NewTicket:
                    amount = 50;
                    TransactionDTO transactionDTO = new TransactionDTO()
                    {
                        CardNumber = Helper.CerasisCardId,
                        Amount = amount,
                        PurposeOfUse = "Mein HTL Wien West - Ticketeröffnung"
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        
                        client.DefaultRequestHeaders.Add("APIToken", Helper.CerasisToken);
                        using HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7260/api/create-transaction", transactionDTO);
                        {
                            try
                            {
                                response.EnsureSuccessStatusCode();
                                string responseBody = await response.Content.ReadAsStringAsync();

                                AES encryption = new AES();
                                string statusMessage = responseBody;

                                return "OK";
                            }
                            catch (Exception ex)
                            {
                                return ex.ToString();
                            }

                        }

                    }
                default:
                    return "";
            }
        }
    }

    public enum TransactionReason
    {
        NewTicket
    }

    public class TransactionDTO
    {
        public string CardNumber { get; set; }
        public double Amount { get; set; }
        public string PurposeOfUse { get; set; }
    }
}
