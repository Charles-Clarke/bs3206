using BS3206.Services;

namespace BS3206.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage() => InitializeComponent();

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var name = FullNameEntry.Text;
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;

        bool success = await AuthService.RegisterUserAsync(name, email, password);
        StatusLabel.Text = success ? "Registration successful!" : "Error creating account.";
    }

    private async void OnBackToLogin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Login");
    }
}
