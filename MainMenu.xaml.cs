using blitz_chat.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace blitz_chat;

public partial class MainMenu : ContentPage
{
    public Dictionary<string, Prijateljstva> prijateljstva = new Dictionary<string, Prijateljstva>();
    public Dictionary<string, Uporabnik> prijatelji = new Dictionary<string, Uporabnik>();
    public MainMenu()
	{
        DisplayPrijateljstva();
        InitializeComponent();
	}

    public async void DisplayPrijateljstva()
    {
        var UserID = await SecureStorage.Default.GetAsync("UserID");
        IFirebaseClient client = new FireSharp.FirebaseClient(new FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app"
        });


        FirebaseResponse response = await client.GetAsync("Prijateljstva");
        prijateljstva = response.ResultAs<Dictionary<string, Prijateljstva>>();

        FirebaseResponse response2 = await client.GetAsync("Uporabniki");
        prijatelji = response2.ResultAs<Dictionary<string, Uporabnik>>();

        if (prijateljstva != null)
        {
            foreach (var prijatelj in prijateljstva)
            {
                string prijateljstvaKey = prijatelj.Key;
                Prijateljstva prijateljstvo = prijatelj.Value;

                if ((prijatelj.Value.uid1 == UserID.ToString() || prijatelj.Value.uid2 == UserID.ToString()) && prijatelj.Value.IsFriendConfirmed == true)
                {
                    StackLayout labelContainer = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };

                    foreach (var user in prijatelji)
                    {
                        if ((user.Key.ToString() == prijatelj.Value.uid1 || user.Key.ToString() == prijatelj.Value.uid2) && user.Key.ToString() != UserID.ToString())
                        {
                            Label label = new Label
                            {
                                Text = user.Value.Email.ToString(),
                                FontSize = 18,
                                TextColor = Color.FromRgb(0, 0, 0),
                                Padding = 10
                            };

                            label.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(async () =>
                                {
                                    await Navigation.PushAsync(new ChatPage(prijateljstvaKey, UserID, user.Key.ToString()));
                                })
                            });

                            labelContainer.Children.Add(label);
                            this.labelContainer.Children.Add(labelContainer);
                        }
                    }
                }
            }
        }

    }

    private async void OnIzpisButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Pozor!", "Se res želite izpisati?", "Da", "Ne");

        if (result)
        {
            await SecureStorage.Default.SetAsync("UserID", null);
            await Navigation.PushAsync(new MainPage());
        }
    }

    private async void BlitzchatClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainMenu());
    }
    private async void OnProfileTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnPrijateljiTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrijateljiPage());
    }
}