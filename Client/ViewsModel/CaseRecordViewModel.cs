using Client.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.ViewsModel
{
	internal class CaseRecordViewModel : ReactiveObject
	{
		private readonly HttpClient _httpClient = new HttpClient();

		private string _filterString;

		public string FilterString
		{
			get => _filterString;
			set => this.RaiseAndSetIfChanged(ref _filterString, value);
		}

		public ObservableCollection<CaseRecord> Countries { get; } = new();

		public ReactiveCommand<Unit, Unit> LoadCommand { get; }
		public ReactiveCommand<Unit, Unit> SearchCommand { get; }

		private List<CaseRecord> _allCountries = new();

		public CaseRecordViewModel()
		{
			LoadCommand = ReactiveCommand.CreateFromTask(LoadCountriesAsync);
			SearchCommand = ReactiveCommand.Create(FilterCountries);

			LoadCommand.Execute().Subscribe();
		}

		private async Task LoadCountriesAsync()
		{
			var response = await _httpClient.GetAsync("https://localhost:7147/api/v2/CaseRecord");
			var json = await response.Content.ReadAsStringAsync();

			var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			_allCountries = JsonSerializer.Deserialize<List<CaseRecord>>(json, opts)!;

			Countries.Clear();
			foreach (var c in _allCountries)
				Countries.Add(c);
		}

		private void FilterCountries()
		{
			var filtered = _allCountries
				.Where(c => string.IsNullOrWhiteSpace(FilterString) ||
							c.Country.StartsWith(FilterString, StringComparison.OrdinalIgnoreCase) ||
							c.Country.EndsWith(FilterString, StringComparison.OrdinalIgnoreCase))
				.ToList();

			Countries.Clear();
			foreach (var c in filtered)
				Countries.Add(c);
		}
	}
}