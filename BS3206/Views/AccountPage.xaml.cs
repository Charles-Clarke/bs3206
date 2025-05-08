using Microsoft.Maui.Storage;
using System.IO;
using BS3206.Services;
using BS3206.Helpers;

namespace BS3206.Views;

public partial class AccountPage : ContentPage
{
    string userEmail => UserSession.Email;

    public AccountPage()
    {
        InitializeComponent();
        LoadProfileImageAsync();
    }

    private async void LoadProfileImageAsync()
    {
        try
        {
            string email = Preferences.Get("UserEmail", null);
            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Email not found in Preferences", "OK");
                return;
            }

            var base64 = await AuthService.GetProfilePictureAsync(email);
            if (!string.IsNullOrEmpty(base64))
            {
                byte[] imageBytes = Convert.FromBase64String(base64);
                ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            else
            {
                // Use default
                ProfileImage.Source = "default_avatar.png";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Crash", $"Failed to load profile image: {ex.Message}", "OK");
        }
    }


    private async void OnUpdatePictureClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select a profile picture",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            using var stream = await result.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var base64 = Convert.ToBase64String(imageBytes);

            // Save to database
            string email = Preferences.Get("UserEmail", null);
            await DisplayAlert("Debug", $"Email from Preferences: {email}", "OK");

            if (email != null)
            {
                await AuthService.UpdateProfilePictureAsync(email, base64);
                ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                await DisplayAlert("Done", "Profile picture updated!", "OK");
            }
        }
    }


}
