using Avalonia.Controls;
using Client.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client;

public partial class LoginView : Window
{
	public LoginView()
	{
		InitializeComponent();
		DataContext = new LoginViewModel(this);
	}
}
