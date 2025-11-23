using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
	public class ApiService
	{
		private readonly HttpClient _client;

		private const string BaseUrl = "https://localhost:7147/api/v2/CaseRecord";


		public ApiService()
		{
			_client = new HttpClient();
		}

		public async Task<CaseRecord?> GetByCountryNameAsync(string country)
		{
			var url = $"{BaseUrl}/Name/{country}";

			var resp = await _client.GetAsync(url);

			if (!resp.IsSuccessStatusCode)
			{
				return null;
			}

			var json = await resp.Content.ReadAsStringAsync();
			var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			return JsonSerializer.Deserialize<CaseRecord>(json, opts);
		}

		public async Task<List<CaseRecord>> GetAllAsync()
		{
			var resp = await _client.GetAsync(BaseUrl);
			resp.EnsureSuccessStatusCode();
			var json = await resp.Content.ReadAsStringAsync();
			var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			return JsonSerializer.Deserialize<List<CaseRecord>>(json, opts) ?? new List<CaseRecord>();
		}


		public async Task<CaseRecord?> GetByIdAsync(int id)
		{
			var resp = await _client.GetAsync($"{BaseUrl}/{id}");
			if (!resp.IsSuccessStatusCode) return null;
			var json = await resp.Content.ReadAsStringAsync();
			var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			return JsonSerializer.Deserialize<CaseRecord>(json, opts);
		}


		public async Task<bool> CreateAsync(CaseRecord record)
		{
			var json = JsonSerializer.Serialize(record);
			var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			var resp = await _client.PostAsync(BaseUrl, content);
			return resp.IsSuccessStatusCode;
		}


		public async Task<bool> UpdateAsync(int id, CaseRecord record)
		{
			var json = JsonSerializer.Serialize(record);
			var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			var resp = await _client.PutAsync($"{BaseUrl}/{id}", content);
			return resp.IsSuccessStatusCode;
		}


		public async Task<bool> DeleteAsync(int id)
		{
			var resp = await _client.DeleteAsync($"{BaseUrl}/{id}");
			return resp.IsSuccessStatusCode;
		}
	}
}
