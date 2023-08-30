using FreshApp.Models;

namespace FreshApp.Views;

public partial class ServicesMenuPage : ContentPage
{
    public ApiService api { get; set; }
    public User User { get; set; }
    public ServicesMenuPage()
	{
		InitializeComponent();

        api = new ApiService();
        User = App.User;

        usernameLabel.Text = $"Welcome {User.Name}";
        CartCountLabel.Text = $"Cart ({User.CartCount})";

        loadListView();
    }

    async void loadListView()
    {
        var res = await api.GetServiceTypes();

        if (res != null)
        {
            ServiceTypesListView.ItemsSource = res;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //App.Current.MainPage = new NavigationPage(new ServicesMenuPage());
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new ShoppingCartPage());
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AboutPage());
    }

    private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        var serviceType = (ServiceType)(sender as Grid).BindingContext;

        App.Current.MainPage = new NavigationPage(new ServiceSelectionPage(serviceType));
    }
}