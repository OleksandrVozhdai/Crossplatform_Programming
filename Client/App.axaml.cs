using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Avalonia.Threading;
using System.Reactive.Concurrency;

namespace Client
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			RxApp.MainThreadScheduler = new DispatcherScheduler(Avalonia.Threading.Dispatcher.UIThread);
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new LoginView(); // запуск логіну
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
