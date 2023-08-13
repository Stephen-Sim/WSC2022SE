namespace FreshApp.Views;

public partial class MainPage : ContentPage
{
	ApiService apiService { get; set; }
	public MainPage()
	{
		InitializeComponent();

        apiService = new ApiService();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var res = await apiService.Login(usernameEntry.Text, passwordEntry.Text);

        if (res)
        {
            App.Current.MainPage = new NavigationPage(new PropertyListingForm());
        }
        else
        {
            await DisplayAlert("", "login failed", "ok");
        }
    }
}