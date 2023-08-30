using FreshApp.Models;

namespace FreshApp.Views;

public partial class ShoppingCartPage : ContentPage
{
    public Coupon coupon { get; set; }

    private int count = 0;
    public decimal price { get; set; }
    public ShoppingCartPage()
    {
        InitializeComponent();

        api = new ApiService();

        price = 0.0m;

        loadListView();
    }

    public ApiService api { get; set; }

    private async void loadListView()
    {
        CartCountLabel.Text = $"Cart ({App.User.CartCount})";
        var res = await api.getAddonServiceDetails(App.User.CartID);

        if (res != null)
        {
            CartListView.ItemsSource = res;

            price = res.Sum(x => x.Price);
            count = res.Count;

            PayAmountLabel.Text = $"Total Amount Payable ({count} items): $ {price.ToString("0.00")}";
        }
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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Alert", "Are you sure to delete this booking?", "yes", "no");

        if (result)
        {
            var id = (long)(sender as Button).CommandParameter;

            var res = await api.delAddonServiceDetail(id);

            if (res)
            {
                await DisplayAlert("Alert", "Booking deleted", "ok");
                CartCountLabel.Text = $"Cart ({App.User.CartCount})";
                loadListView();
            }
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var res = await api.checkCoupon(CouponEntry.Text);

        if (res != null)
        {
            coupon = res;
            IsSucceedAppliedLabel.Text = "Coupon Successully Applied!";

            decimal discountPrice = price * (coupon.DiscountPercent / 100);

            if (discountPrice > coupon.MaximimDiscountAmount)
            {
                discountPrice = coupon.MaximimDiscountAmount;
            }

            PayAmountLabel.Text = $"Total Amount Payable ({count} items): $ {(price - discountPrice).ToString("0.00")}";
        }
        else
        {
            IsSucceedAppliedLabel.Text = "Invalid Coupon";
        }
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        if (count == 0)
        {
            await DisplayAlert("Alert", "No service to pay", "Ok");
            return;
        }

        if (coupon == null)
        {
            await DisplayAlert("Alert", "Please insert coupon.", "Ok");
            return;
        }

        var result = await DisplayAlert("Alert", "Are you sure to Proceed payment?", "yes", "no");

        if (result)
        {
            var res = await api.proceedPayment(App.User.CartID, coupon.ID);

            if (res)
            {
                await DisplayAlert("Alert", "Payment Success.", "Ok");
                CouponEntry.Text = string.Empty;
                loadListView();
            }
        }
    }
}