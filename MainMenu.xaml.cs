namespace blitz_chat;

public partial class MainMenu : ContentPage
{
	public MainMenu()
	{
		InitializeComponent();
	}

    private void OnIzpisButtonClicked(object sender, EventArgs e)
    {

    }

    private async void OnProfileTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private void OnPrijateljiTabClicked(object sender, EventArgs e)
    {

    }
}