using Firebase.Auth;
using Firebase.Database;
using Microsoft.Maui.ApplicationModel;
using Google.Cloud.Firestore;
using FirebaseAdmin;
using Firebase.Database.Query;
using blitz_chat.Models;
using System.Text.RegularExpressions;

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
        if (PasswordEntry.Text != null & EmailEntry.Text != null)
        {
            if (Regex.IsMatch(EmailEntry.Text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$") & Regex.IsMatch(PasswordEntry.Text, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"))
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
                await DisplayAlert("Napaka!", "Niste vnesli email-a ali geslo z 8 znakov in vsaj 1 številko!", "OK");
            }

        }
        else
        {
            await DisplayAlert("Napaka!", "Polja nesmejo biti prazna!", "OK");
        }


    }
}