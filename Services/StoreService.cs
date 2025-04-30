using ESIID42025.Models;
using System.Net.Http.Json;

public class StoreService
{
    private readonly HttpClient _httpClient;

    public StoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Store>> GetStoresAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Store>>("api/Store")
               ?? new List<Store>();
    }

    public async Task<Store?> GetStoreByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Store>($"api/Store/{id}");
    }

    public async Task CreateStoreAsync(Store store)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Store", store);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create store: {response.StatusCode} - {content}");
        }
    }

    public async Task UpdateStoreAsync(Store store)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Store/{store.ID}", store);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update store: {response.StatusCode} - {content}");
        }
    }

    public async Task DeleteStoreAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Store/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete store: {response.StatusCode} - {content}");
        }
    }
}