using DiseaseOutbreaksMobile.Services; // <-- FIX: Added this line to find AuthService

namespace DiseaseOutbreaksMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add our service as a singleton
        builder.Services.AddSingleton<AuthService>();

        return builder.Build();
    }
}