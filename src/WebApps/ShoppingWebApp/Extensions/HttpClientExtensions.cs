using System.Net.Http.Headers;
using System.Text.Json;

namespace ShoppingWebApp.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
        }
        var dataAsString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public static async Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return  await httpClient.PostAsync(url, content);        
    }

    public static async Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await httpClient.PutAsync(url, content);        
    }
}
