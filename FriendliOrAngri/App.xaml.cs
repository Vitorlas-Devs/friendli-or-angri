namespace FriendliOrAngri;

public partial class App : Application
{
    static Database database;

    public static Database Database
    {
        get
        {
            if (database == null)
            {
                database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FriendliOrAngri.db3"));
            }
            return database;
        }
    }
    public App()
	{
		InitializeComponent();
        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();
	}
}
