namespace blitz_chat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new ViewModels.VpisViewModel(Navigation);
        }

        private async void Register_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
        }
    }
}