using Avalonia.Controls;
using System.Threading.Tasks;

namespace Client
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async Task ShowLoadingScreen()
		{
			var lv = new LoadingView();
			lv.Show();

			await Task.Delay(500);

			lv.Close();
		}

		private async void HistoryButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			await ShowLoadingScreen();
			new CaseRecordView().Show();
		}

		private async void AboutButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			new About().Show();
		}

		private async void TableButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			await ShowLoadingScreen();
			new CentralTable().Show();
		}
	}
}
