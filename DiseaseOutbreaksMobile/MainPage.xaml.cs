using DiseaseOutbreaksMobile.Services;

namespace DiseaseOutbreaksMobile;

public partial class MainPage : ContentPage
{
    private readonly AuthService _authService;

    // Dependency injection automatically provides AuthService
    public MainPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        // Make sure the CounterBtn has a Clicked event handler configured in MainPage.xaml
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var result = await _authService.LoginAsync();
        
        // This is a common MAUI control, assumed to exist in MainPage.xaml
        if (!result.IsError)
        {
            // FIX: Use safe access operators (?.) and null coalescing (??) 
            // to safely get the user's name and eliminate warning CS8602.
            string userName = result.User?.Identity?.Name ?? "Authenticated User";
            CounterBtn.Text = $"Привет, {userName}!";
        }
        else
        {
            CounterBtn.Text = $"Ошибка: {result.Error}";
        }
    }
}