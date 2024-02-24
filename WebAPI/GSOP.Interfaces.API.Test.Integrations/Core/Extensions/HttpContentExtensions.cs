using Newtonsoft.Json;

namespace GSOP.Interfaces.API.Test.Integrations.Core.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T?> ReadObjectAsync<T>(this HttpContent httpContent)
        {
            var responseContent = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
