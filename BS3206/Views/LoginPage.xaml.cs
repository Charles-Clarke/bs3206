using BS3206.Helpers;
using BS3206.Services;

namespace BS3206.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage() => InitializeComponent();

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;

        bool success = await AuthService.ValidateLoginAsync(email, password);
        if (success)
        {
            Preferences.Set("UserEmail", email);
            await Shell.Current.GoToAsync("//Dashboard");
        }
        else
        {
            StatusLabel.Text = "Invalid credentials";
        }
    }

    private async void OnGoToRegister(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Register");
    }
}
