namespace FriendliOrAngri;

public partial class AppShell : Shell
{
    public static AppShell instance;
    
    public AppShell()
	{
		InitializeComponent();
        instance = this;
    }

    public void DisableTabs(string currentTab)
    {
        if (currentTab == "normal")
        {
            tab1.IsEnabled = false;
            tab2.IsEnabled = false;
            tab3.IsEnabled = true;
            tab4.IsEnabled = false;
            tab5.IsEnabled = false;
        }
        else
        {
            tab1.IsEnabled = false;
            tab2.IsEnabled = false;
            tab3.IsEnabled = false;
            tab4.IsEnabled = true;
            tab5.IsEnabled = false;
        }
    }

    public void EnableTabs()
    {
        tab1.IsEnabled = true;
        tab2.IsEnabled = true;
        tab3.IsEnabled = true;
        tab4.IsEnabled = true;
        tab5.IsEnabled = true;
    }
}
