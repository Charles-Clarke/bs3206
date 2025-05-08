namespace BS3206.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
	}
    private async void OnAccountButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AccountPage");
    }

}