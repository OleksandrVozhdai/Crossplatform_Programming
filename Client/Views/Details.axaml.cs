using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Models;
using Client.Services;
using Client.ViewsModel;

namespace Client;

public partial class Details : Window
{
	private readonly ApiService _apiService = new ApiService();

	public Details(CaseRecord record)
	{
		InitializeComponent();

		DataContext = new DetailsViewModel(_apiService, record);
	}
}