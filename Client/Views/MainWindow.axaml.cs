using Avalonia.Controls;
using System;
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

			Random rnd = new Random();
			int delay = rnd.Next(100, 601);

			await Task.Delay(delay);

			lv.Close();
		}

		private async void HistoryButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			await ShowLoadingScreen();
			new CaseRecordView().Show();
		}

		private void AboutButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
