using Firebase.Auth;
using Firebase.Database;
using Microsoft.Maui.ApplicationModel;
using Google.Cloud.Firestore;
using FirebaseAdmin;
using Firebase.Database.Query;
using blitz_chat.Models;
using UporabniskiVmesnik.Models;
namespace blitz_chat;

public partial class RegistrationPage : ContentPage
{
    public FirebaseClient firebase;
    public RegistrationPage()
    {
        InitializeComponent();
        FirebaseContext firebaseContext = new FirebaseContext();
        firebaseContext.InitializeFirebase();
        firebase = firebaseContext.firebase;
    }

    public async Task RegisterUserToFirebase(Uporabnik uporabnik)
    {
        try
        {
            var firebaseRoot = firebase.Child("Uporabniki");
            await firebaseRoot.PostAsync(uporabnik);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to Firebase: {ex.Message}");
        }
    }
    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
        if (EmailEntry.Text != null & PasswordEntry.Text != null)
        {
            if (PasswordEntry.Text == PasswordEntry2.Text)
            {
                Uporabnik user = new Uporabnik
                {
                    Email = EmailEntry.Text,
                    Geslo = PasswordEntry.Text,
                    Profilna = "profilna1.png",
                    Status = "Nothing here"
                };
                await RegisterUserToFirebase(user);
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Napaka!", "Gesla se morata ujemati!", "OK");

            }
        }
        else
        {
            await DisplayAlert("Napaka!", "Polja nesmejo biti prazna!", "OK");
        }

    }
}