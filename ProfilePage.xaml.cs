using FireSharp.Interfaces;
using FireSharp.Response;
using blitz_chat.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Firebase.Auth;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace blitz_chat;

public partial class ProfilePage : ContentPage
{
    public FirebaseClient firebase;
    public string profileImage = null;
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
        flushCache();

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
        profileImage = uporabnik.Profilna.ToString();
    }

    public void flushCache()
    {
        var cacheDir = FileSystem.CacheDirectory;
        if (Directory.Exists(cacheDir))
        {
            Directory.Delete(cacheDir, true);
        }

        var imageManagerDiskCache = Path.Combine(FileSystem.CacheDirectory, "image_manager_disk_cache");

        if (Directory.Exists(imageManagerDiskCache))
        {
            foreach (var imageCacheFile in Directory.EnumerateFiles(imageManagerDiskCache))
            {
                Debug.WriteLine($"Deleting {imageCacheFile}");
                File.Delete(imageCacheFile);
            }
        }
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

    async Task SaveImageTodb(string FullPath)
    {
        string UserID = await SecureStorage.Default.GetAsync("UserID");
        var stream = File.Open(FullPath, FileMode.Open);

        var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE"));
        var a = await auth.SignInWithEmailAndPasswordAsync("admin@admin.com", "admin1");

        var contentType = "image/png";
        CancellationToken ct = new CancellationToken();

        var task = new FirebaseStorage(
            "blitzchat-4a405.appspot.com",
             new FirebaseStorageOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                 ThrowOnCancel = true,
             })
            .Child("Profilne")
            .Child(UserID)
            .PutAsync(stream, ct, contentType);

        return;
    }

    async Task ImagePickerTapped()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick Image"
            });

            if(result != null)
            {
                await SaveImageTodb(result.FullPath.ToString());
                string UserID = await SecureStorage.Default.GetAsync("UserID");
                ProfileImage.Source = "https://firebasestorage.googleapis.com/v0/b/blitzchat-4a405.appspot.com/o/Profilne%2F" + UserID + "?alt=media";

                flushCache();
            }
            
            return;
        }
        catch(Exception ex)
        {
            await DisplayAlert("Napaka!", "Datoteka ni vredu", "OK");
        }

    }

    private void OnEditIconClicked(object sender, EventArgs e)
    {
        ProfileImage.Source = "profilna_edit1.png";
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
        string UserID = await SecureStorage.Default.GetAsync("UserID");

        IFirebaseClient client = new FireSharp.FirebaseClient(new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app"
        });


        FirebaseResponse response = await client.GetAsync("Uporabniki/" + UserID);
        Uporabnik uporabnik = response.ResultAs<Uporabnik>();

        Uporabnik user = new Uporabnik
        {
            Geslo = uporabnik.Geslo,
            Email = email.Text,
            Profilna = "https://firebasestorage.googleapis.com/v0/b/blitzchat-4a405.appspot.com/o/Profilne%2F"+UserID+"?alt=media",
            Status = status.Text
        };

        try
        {
            var firebaseRoot = firebase.Child("Uporabniki").Child(UserID);
            await firebaseRoot.PutAsync(user);

            UserPodatki();
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

    private async void ChangeImageClicked(object sender, EventArgs e)
    {
        if(ProfileImage.Source.ToString() == "File: profilna_edit1.png")
        {
            await ImagePickerTapped();
        }
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