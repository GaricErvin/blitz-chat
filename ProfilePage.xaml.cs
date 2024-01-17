namespace blitz_chat;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void BlitzchatClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainMenu());
    }


    private async void OnIzpisButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Pozor!", "Se res želite izpisati?", "Da", "Ne");

        if (result)
        {
            //Uporabnika izpise, vrne na prijavo
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            //Nic se ne zgodi
        }

    }

    private async void OnPrijateljiTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrijateljiPage());
    }

    private void OnEditIconClicked(object sender, EventArgs e)
    {
        ProfileImage.Source = "profilna_edit1.png";
        CircleFrame.BackgroundColor = Color.FromRgb(211, 211, 211);


        EditIcon.IsVisible = false;
        username.IsEnabled = true;
        email.IsEnabled = true;
        status.IsEnabled = true;
        SaveIcon.IsVisible = true;
        DisbandIcon.IsVisible = true;
    }

    private void OnSaveIconClicked(object sender, EventArgs e)
    {
        //TO DO: shrani na novo vpisane podatke in slike v bazo

        //staticno:
        ProfileImage.Source = "profilna1.png";

        CircleFrame.BackgroundColor = Color.FromRgb(255, 255, 255);
        EditIcon.IsVisible = true;
        username.IsEnabled = false;
        email.IsEnabled = false;
        status.IsEnabled = false;
        SaveIcon.IsVisible = false;
        DisbandIcon.IsVisible = false;
    }

    private void OnDisbandIconClicked(object sender, EventArgs e)
    {
        //TO DO: vrne vse na stanje pred klikom na edit

        //staticno:
        ProfileImage.Source = "profilna1.png";
        username.Text = "jozkar93845";
        email.Text = "joze.grindogardar@gmail.com";
        status.Text = "Doing homework! :D";

        CircleFrame.BackgroundColor = Color.FromRgb(255, 255, 255);
        EditIcon.IsVisible = true;
        username.IsEnabled = false;
        email.IsEnabled = false;
        status.IsEnabled = false;
        SaveIcon.IsVisible = false;
        DisbandIcon.IsVisible = false;
    }

}