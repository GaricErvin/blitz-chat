using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using blitz_chat.Models;
using Microsoft.Maui.Storage;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Firebase.Auth;

namespace blitz_chat;

public partial class ProfilePage : ContentPage
{
    public FirebaseClient firebase;
    public ProfilePage()
    {
        FirebaseContext firebaseContext = new FirebaseContext();
        firebaseContext.InitializeFirebase();
        firebase = firebaseContext.firebase;
        UserPodatki();
        InitializeComponent();
    }

    private async void UserPodatki()
    {
        string UserID = await SecureStorage.Default.GetAsync("UserID");

        IFirebaseClient client = new FireSharp.FirebaseClient(new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app"
        });


        FirebaseResponse response = await client.GetAsync("Uporabniki/"+UserID);
        Uporabnik uporabnik = response.ResultAs<Uporabnik>();

        email.Text = uporabnik.Email.ToString();
        status.Text = uporabnik.Status.ToString();
        ProfileImage.Source = uporabnik.Profilna.ToString();
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
            await SecureStorage.Default.SetAsync("UserID", null);
            await Navigation.PushAsync(new MainPage());
        }
    }

    private async void OnPrijateljiTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrijateljiPage());
    }

    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }

    async Task SaveImageTodb(string photopath)
    {
        string UserID = await SecureStorage.Default.GetAsync("UserID");
        var stream = File.Open(photopath, FileMode.Open);

        var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE"));
        var a = await auth.SignInWithEmailAndPasswordAsync("admin@admin.com", "admin1");

        var task = new FirebaseStorage(
            "blitzchat-4a405.appspot.com",
             new FirebaseStorageOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                 ThrowOnCancel = true,
             })
            .Child("Profilne")
            .Child(UserID)
            .PutAsync(stream);

        var downloadUrl = await task;


    }

    async Task Image_Picker_Tapped()
    {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick Image"
            });

        await SaveImageTodb(result.FullPath.ToString());
    }

    private void OnEditIconClicked(object sender, EventArgs e)
    {
        Image_Picker_Tapped();
        //ProfileImage.Source = "profilna_edit1.png";
        CircleFrame.BackgroundColor = Color.FromRgb(211, 211, 211);


        EditIcon.IsVisible = false;
        username.IsEnabled = true;
        email.IsEnabled = true;
        status.IsEnabled = true;
        SaveIcon.IsVisible = true;
        DisbandIcon.IsVisible = true;
    }

    private async void OnSaveIconClicked(object sender, EventArgs e)
    {
        ProfileImage.Source = "profilna1.png";

        string UserID = await SecureStorage.Default.GetAsync("UserID");

        IFirebaseClient client = new FireSharp.FirebaseClient(new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app"
        });


        FirebaseResponse response = await client.GetAsync("Uporabniki/" + UserID);
        Uporabnik uporabnik = response.ResultAs<Uporabnik>();

        string source = ProfileImage.Source.ToString();
        string filetrim = "File: ";
        string Profilna = source.Substring(filetrim.Length);

        Uporabnik user = new Uporabnik
        {
            Geslo = uporabnik.Geslo,
            Email = email.Text,
            Profilna = Profilna,
            Status = status.Text
        };

        try
        {
            var firebaseRoot = firebase.Child("Uporabniki").Child(UserID);
            await firebaseRoot.PutAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to Firebase: {ex.Message}");
        }

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
        UserPodatki();
        CircleFrame.BackgroundColor = Color.FromRgb(255, 255, 255);
        EditIcon.IsVisible = true;
        username.IsEnabled = false;
        email.IsEnabled = false;
        status.IsEnabled = false;
        SaveIcon.IsVisible = false;
        DisbandIcon.IsVisible = false;
    }

}