using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blitz_chat.ViewModels
{
    class RegistrationViewModel
    {
        public string webApiKey = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE";
        private INavigation _navigation;

        public Command RegistracijaUporabnika { get; }

        public string email { get; set; }

        public string geslo { get; set; }
        public string username { get; set; }

        public RegistrationViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegistracijaUporabnika = new Command(RegistracijaUporabnikaTappedAsync);
        }
        private async void RegistracijaUporabnikaTappedAsync(object obj)
        {
            try
            {
                FirebaseAuthProvider ponudnikAvtorizacije = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                FirebaseAuthLink auth = await ponudnikAvtorizacije.CreateUserWithEmailAndPasswordAsync(email, geslo);
                string token = auth.FirebaseToken;

                if (token != null)
                {
                    await App.Current.MainPage.DisplayAlert("Obvestilo", "Uporabnik uspešno registriran", "OK");
                    await this._navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Napaka!", "Prosimo vnesite pravilen email naslov!", "OK");
            }

        }
    }
}
