using CommunityToolkit.Maui;

namespace FriendliOrAngri;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa_solid.ttf", "FontAwesome");
            })
            .UseMauiCommunityToolkit();
        // no idea found these in the microsoft docs
        //.Services.AddSingleton<Database>()
        //.AddTransient<Database>();

        return builder.Build();
    }
}
