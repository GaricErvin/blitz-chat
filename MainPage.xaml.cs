namespace blitz_chat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Button registracijaButton = (Button)FindByName("RegistracijaButton");
            registracijaButton.Clicked += OnRegistracijaButtonClicked;

            Button loginButton = (Button)FindByName("LoginButton");
            loginButton.Clicked += OnLoginButtonClicked;
        }

        private async void OnRegistracijaButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
        }
    }
}