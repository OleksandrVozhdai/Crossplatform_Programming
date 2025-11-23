using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Models;
using Client.Services;
using Client.ViewsModel;
using System;
using System.Threading.Tasks;

namespace Client;

public partial class CaseRecordView : Window
{
	public CaseRecordView()
	{
		InitializeComponent();
		DataContext = new CaseRecordViewModel();
	}

	private async Task ShowLoadingScreen()
	{
		var lv = new LoadingView();
		lv.Show();

		await Task.Delay(500);

		lv.Close();
	}

	private async void Button_Click_Details(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{

		if (sender is Button btn && btn.DataContext is CaseRecord record)
		{
			var dw = new Details(record);
			await ShowLoadingScreen();
			dw.Show();
		}
	}
}