namespace blitz_chat;

public partial class PrijateljiPage : ContentPage
{
	public PrijateljiPage()
	{
		InitializeComponent();
	}

    private async void BlitzchatClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainMenu());

    }

    private async void OnIzpisButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Pozor!", "Ste prepri?ani da se želite izpisati?", "Da", "Ne");

        if (result)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {

        }
    }

    private async void OnProfileTab(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}