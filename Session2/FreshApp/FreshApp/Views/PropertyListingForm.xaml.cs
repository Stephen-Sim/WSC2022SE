namespace FreshApp.Views;

public partial class PropertyListingForm : ContentPage
{
    public int user_id { get; set; }


    public PropertyListingForm()
	{
		InitializeComponent();
		this.user_id = Preferences.Get("user_id", 0);
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Clear();

        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}