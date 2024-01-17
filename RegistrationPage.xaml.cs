using Firebase.Auth;
using Firebase.Database;
using Microsoft.Maui.ApplicationModel;
using UporabniskiVmesnik.ViewModels;
using Google.Cloud.Firestore;
using FirebaseAdmin;
using Firebase.Database.Query;


namespace blitz_chat;

public partial class RegistrationPage : ContentPage
{
    private readonly FirebaseClient firebase;
    public static string firebaseUrl = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app/";

    public RegistrationPage()
    {
        InitializeComponent();
        firebase = InitializeFirebase();
        //BindingContext = new ViewModels.RegistrationViewModel(Navigation);
    }

    private FirebaseClient InitializeFirebase()
    {
        return new FirebaseClient(firebaseUrl);
    }

    public async Task WriteToFirebase(Uporabnik uporabnik)
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
        var user = new Uporabnik
        {
            Email = EmailEntry.Text,
            Geslo = PasswordEntry.Text,
            Profilna = "profilna1.png",
            Status = "Nothing here"
        };
        await WriteToFirebase(user);
    }
}