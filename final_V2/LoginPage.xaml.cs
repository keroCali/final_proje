namespace final_V2;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        var user = await App.Database.LoginAsync(username, password);
        if (user != null)
        {
            App.CurrentUser = user;
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Hata", "Geçersiz kullanıcı adı veya şifre.", "Tamam");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        var existingUser = await App.Database.LoginAsync(username, password);
        if (existingUser != null)
        {
            await DisplayAlert("Hata", "Bu kullanıcı zaten mevcut.", "Tamam");
            return;
        }

        var newUser = new User { Username = username, Password = password };
        await App.Database.RegisterAsync(newUser);
        await DisplayAlert("Başarılı", "Kayıt olundu. Giriş yapabilirsiniz.", "Tamam");
    }
}
