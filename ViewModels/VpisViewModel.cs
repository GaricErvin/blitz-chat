using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blitz_chat.ViewModels
{
    class VpisViewModel
    {
        public string webApikey = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE";
        private INavigation _navigation;
        public Command RegistracijaBtn { get; }
        public Command PrijavaBtn { get; }
        public string email { get; set; }
        public string geslo { get; set; }
        public string username { get; set; }
        public VpisViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            RegistracijaBtn = new Command(RegistracijaBtnTappedAsync);
            PrijavaBtn = new Command(PrijavaBtnTappedAsync);
        }
        private async void RegistracijaBtnTappedAsync(object obj)
        {
            await this._navigation.PushAsync(new RegistrationPage());
        }
        private async void PrijavaBtnTappedAsync(object obj)
        {
            FirebaseAuthProvider ponudnikAvtorizacije = new FirebaseAuthProvider(new FirebaseConfig(webApikey));
            try
            {
                FirebaseAuthLink auth = await ponudnikAvtorizacije.SignInWithEmailAndPasswordAsync(email, geslo);
                FirebaseAuthLink vsebina = await auth.GetFreshAuthAsync();
                var serializiranaVsebina = JsonConvert.SerializeObject(vsebina);
                Preferences.Set("SvezToken", serializiranaVsebina);
                await this._navigation.PushAsync(new MainMenu());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Napaka!", "Napacen email ali geslo!", "OK");
            }
        }
    }
}
