using Firebase.Database;
using blitz_chat.Models;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp.Config;

namespace blitz_chat
{
    public partial class MainPage : ContentPage
    {
        public FirebaseClient firebase;
        public static Dictionary<string, Uporabnik> Uporabniki = new Dictionary<string, Uporabnik>();
        public MainPage()
        {
            InitializeComponent();
            FirebaseContext firebaseContext = new FirebaseContext();
            firebaseContext.InitializeFirebase();
            firebase = firebaseContext.firebase;
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        public async Task FindUser(Uporabnik uporabnik)
        {
            bool logged = false;
            try
            {
                
                IFirebaseClient client = new FireSharp.FirebaseClient(new FirebaseConfig
                {
                    AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
                    BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app"
                });


                FirebaseResponse response = await client.GetAsync("Uporabniki");
                Uporabniki = response.ResultAs<Dictionary<string, Uporabnik>>();

                foreach (var item in Uporabniki)
                {
                    if(item.Value.Email == uporabnik.Email & item.Value.Geslo == uporabnik.Geslo)
                    {
                        await SecureStorage.Default.SetAsync("UserID", item.Key.ToString());
                        await Navigation.PushAsync(new MainMenu());
                        logged = true;
                    }
                }

                if (!logged)
                {
                    await DisplayAlert("Napaka!", "Uporabnik ne obstaja", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem z povezavo na bazo: {ex.Message}");
            }
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (EmailEntry.Text != null & PasswordEntry.Text != null)
            {
                Uporabnik user = new Uporabnik
                {
                    Email = EmailEntry.Text,
                    Geslo = PasswordEntry.Text,
                    Profilna = "profilna1.png",
                    Status = "Nothing here"
                };
                await FindUser(user);
            }
            else
            {
                await DisplayAlert("Napaka!", "Polja nesmejo biti prazna!", "OK");
            }
                
        }
    }
}