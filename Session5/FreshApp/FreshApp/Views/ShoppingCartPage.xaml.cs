using FreshApp.Models;

namespace FreshApp.Views;

public partial class ShoppingCartPage : ContentPage
{
    public ApiService api { get; set; }
    public User User { get; set; }

    public ShoppingCartPage()
    {
        InitializeComponent();

        api = new ApiService();
        User = App.User;

        CartCountLabel.Text = $"Cart ({User.CartCount})";
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new ServicesMenuPage());
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        // App.Current.MainPage = new NavigationPage(new ShoppingCartPage());
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AboutPage());
    }
}