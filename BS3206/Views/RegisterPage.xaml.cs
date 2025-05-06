using BS3206.Helpers;
using BS3206.Services;

namespace BS3206.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage() => InitializeComponent();

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var fullName = FullNameEntry.Text;
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        var confirmPassword = ConfirmPasswordEntry.Text;

        StatusLabel.TextColor = Colors.Red;

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            StatusLabel.Text = "All fields are required.";
            return;
        }

        if (!ValidationHelper.IsValidEmail(email))
        {
            StatusLabel.Text = "Invalid email format.";
            return;
        }

        if (!ValidationHelper.IsValidPassword(password))
        {
            StatusLabel.Text = "Password must be at least 7 characters, include upper/lowercase, a number, and a symbol (!?@#).";
            return;
        }

        if (password != confirmPassword)
        {
            StatusLabel.Text = "Passwords do not match.";
            return;
        }

        if (await AuthService.IsEmailTakenAsync(email))
        {
            StatusLabel.Text = "Email is already registered.";
            return;
        }

        if (await AuthService.IsUsernameTakenAsync(fullName))
        {
            StatusLabel.Text = "Username already in use.";
            return;
        }

        bool registered = await AuthService.RegisterUserAsync(fullName, email, password);

        if (registered)
        {
            StatusLabel.TextColor = Colors.Green;
            StatusLabel.Text = "Registration successful!";
        }
        else
        {
            StatusLabel.Text = "An error occurred. Try again.";
        }
    }

    private async void OnBackToLogin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Login");
    }

    private void OnEmailChanged(object sender, TextChangedEventArgs e)
    {
        if (!ValidationHelper.IsValidEmail(EmailEntry.Text))
            EmailStatus.Text = "Invalid email format.";
        else
            EmailStatus.Text = "";
    }

    private void OnPasswordChanged(object sender, TextChangedEventArgs e)
    {
        if (!ValidationHelper.IsValidPassword(PasswordEntry.Text))
            PasswordStatus.Text = "Too weak: Use 7+ chars, mix case, number, and !?@#";
        else
            PasswordStatus.Text = "";
    }

    private void OnConfirmPasswordChanged(object sender, TextChangedEventArgs e)
    {
        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            ConfirmStatus.Text = "Passwords do not match.";
        else
            ConfirmStatus.Text = "";
    }
    private async void OnEmailUnfocused(object sender, FocusEventArgs e)
    {
        if (!ValidationHelper.IsValidEmail(EmailEntry.Text))
        {
            EmailStatus.Text = "Invalid email format.";
            return;
        }

        bool emailExists = await AuthService.IsEmailTakenAsync(EmailEntry.Text);
        EmailStatus.Text = emailExists ? "Email is already registered." : "";
    }

    private async void OnUsernameUnfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FullNameEntry.Text))
        {
            FullNameStatus.Text = "Username cannot be empty.";
            return;
        }

        bool usernameExists = await AuthService.IsUsernameTakenAsync(FullNameEntry.Text);
        FullNameStatus.Text = usernameExists ? "Username is already in use." : "";
    }


}
