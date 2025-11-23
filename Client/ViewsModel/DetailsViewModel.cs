using Client.Models;
using Client.Services;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client.ViewsModel
{
	internal class DetailsViewModel : ReactiveObject
	{
		private readonly ApiService _apiService;

		public CaseRecord Record { get; }

		// Відображається в UI
		private CaseRecord? _selectedCountryResult;
		public CaseRecord? SelectedCountryResult
		{
			get => _selectedCountryResult;
			set => this.RaiseAndSetIfChanged(ref _selectedCountryResult, value);
		}

		private string _countryNameFilter;
		public string CountryNameFilter
		{
			get => _countryNameFilter;
			set => this.RaiseAndSetIfChanged(ref _countryNameFilter, value);
		}

		public ReactiveCommand<Unit, Unit> SearchCountryCommand { get; }

		// ----------------------------------------------------------
		// ГОЛОВНИЙ ПРАВИЛЬНИЙ КОНСТРУКТОР
		// ----------------------------------------------------------
		public DetailsViewModel(ApiService apiService, CaseRecord record)
		{
			_apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));

			Record = record;
			SelectedCountryResult = record; // одразу показуємо дані, які прийшли зі списку

			// параметр пошуку (опціонально)
			CountryNameFilter = record.Country;

			var canSearch = this
				.WhenAnyValue(x => x.CountryNameFilter)
				.Select(name => !string.IsNullOrWhiteSpace(name));

			SearchCountryCommand =
				ReactiveCommand.CreateFromTask(GetOneCountryAsync, canSearch);
		}

		// ----------------------------------------------------------
		// Запит до API
		// ----------------------------------------------------------
		private async Task GetOneCountryAsync()
		{
			var countryName = CountryNameFilter;

			var result = await _apiService.GetByCountryNameAsync(countryName);

			// UI потік НЕ порушується — ReactiveUI сам викликає це у UI thread
			SelectedCountryResult = result;

			if (result == null)
			{
				Console.WriteLine($"Country '{countryName}' not found.");
			}
		}
	}
}
