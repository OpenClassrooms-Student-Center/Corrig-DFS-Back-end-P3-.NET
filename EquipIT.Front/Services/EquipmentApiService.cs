using System.Net.Http.Json;
using EquipIT.Front.Models;

namespace EquipIT.Front.Services;

public class EquipmentApiService
{
    private readonly HttpClient _http;

    public EquipmentApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Equipment>> GetEquipmentAsync()
    {
        return await _http.GetFromJsonAsync<List<Equipment>>("api/equipment") ?? [];
    }

    public async Task BorrowAsync(int id, string firstName, string lastName)
    {
        var request = new BorrowRequest { FirstName = firstName, LastName = lastName };
        using var response = await _http.PutAsJsonAsync($"api/equipment/{id}/borrow", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }
    }

    public async Task ReturnAsync(int id)
    {
        using var response = await _http.PutAsync($"api/equipment/{id}/return", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }
    }
}
