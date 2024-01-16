namespace blitz_chat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Button registracijaButton = (Button)FindByName("RegistracijaButton");
            registracijaButton.Clicked += OnRegistracijaButtonClicked;
        }

        private async void OnRegistracijaButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the new registration page
            await Navigation.PushAsync(new RegistrationPage());
        }
    }
}