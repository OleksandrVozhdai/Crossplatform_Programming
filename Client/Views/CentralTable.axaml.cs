using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewsModel;

namespace Client;

public partial class CentralTable : Window
{
    public CentralTable()
    {
        InitializeComponent();
		DataContext = new CaseRecordViewModel();
	}
}