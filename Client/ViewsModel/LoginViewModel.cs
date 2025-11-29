using Avalonia.Controls;
using ReactiveUI;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;

namespace Client.ViewsModel
{
	public class LoginViewModel : ReactiveObject
	{
		private readonly Window _loginWindow;

		private string _email = "";
		public string Email
		{
			get => _email;
			set => this.RaiseAndSetIfChanged(ref _email, value);
		}

		private string _errorMessage = "";
		public string ErrorMessage
		{
			get => _errorMessage;
			set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
		}

		public ReactiveCommand<Unit, Unit> LoginCommand { get; }

		public LoginViewModel(Window window)
		{
			_loginWindow = window;
			LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
		}

		private async Task LoginAsync()
		{
			if (string.IsNullOrWhiteSpace(Email))
			{
				Avalonia.Threading.Dispatcher.UIThread.Post(() =>
				{
					ErrorMessage = "Email is required";
				});
				return;
			}

			try
			{
				using var http = new HttpClient();
				var url = $"http://localhost:5000/api/v1/users/login?email={Email}";

				var response = await http.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					Avalonia.Threading.Dispatcher.UIThread.Post(() =>
					{
						ErrorMessage = "User not found!";
					});
					return;
				}

				// відкриваємо нове вікно і закриваємо старе
				Avalonia.Threading.Dispatcher.UIThread.Post(() =>
				{
					var main = new MainWindow();
					main.DataContext = new MainWindowViewModel();
					main.Show();

					_loginWindow.Close();
				});
			}
			catch
			{
				Avalonia.Threading.Dispatcher.UIThread.Post(() =>
				{
					ErrorMessage = "Server error!";
				});
			}
		}

	}
}
