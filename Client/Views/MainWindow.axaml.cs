using Avalonia.Controls;

namespace Client
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void HistoryButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var hw = new CaseRecordView();
			hw.Show();
		}

		private void AboutButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var abw = new About();
			abw.Show();
		}

		private void TableButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			var tw = new CentralTable();
			tw.Show();
		}
	}
}