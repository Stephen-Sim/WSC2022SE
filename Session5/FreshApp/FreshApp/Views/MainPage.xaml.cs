namespace FreshApp.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		api = new ApiService();
	}

	ApiService api { get; set; }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var res = await api.Login(usernameEntry.Text, passwordEntry.Text);

		if (res)
		{
			App.Current.MainPage = new NavigationPage(new ServicesMenuPage());
		}
		else
		{
			await DisplayAlert("", "login failed", "ok");
		}
    }
}