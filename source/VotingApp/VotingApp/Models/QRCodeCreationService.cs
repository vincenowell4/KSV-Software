namespace VotingApp.Models
{
    public class QRCodeCreationService
    {
        static HttpClient client = new HttpClient();
        public async Task<byte[]> CreateQRCode(string url)
        {
            byte[] result = null;
            var response =await client.GetAsync("https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + url);
            if (response.IsSuccessStatusCode)
            {
                result  = await response.Content.ReadAsByteArrayAsync();
            }
            return result;
        }
    }
}
