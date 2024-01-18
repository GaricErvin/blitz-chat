using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using blitz_chat.Models;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace blitz_chat;

public partial class ChatPage : ContentPage
{
    private Timer _timer;
    public string uid1;
    public string uid2;
    public FirebaseClient firebase;
    public string prijateljstvaKey;
    public ChatPage(string a, string UserID, string uid)
    {
        InitializeComponent();
        prijateljstvaKey = a;
        uid1 = UserID;
        uid2 = uid;
        InitializeFirebase();
        _timer = new Timer(RefreshPage, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
    }

    private async void RefreshPage(object state)
    {
        var messages = await GetMessages(firebase, prijateljstvaKey);

        Device.BeginInvokeOnMainThread(() =>
        {
            messagesCollectionView.ItemsSource = messages;
        });
    }

    private async void InitializeFirebase() 
    {
        FirebaseContext firebaseContext = new FirebaseContext();
        firebaseContext.InitializeFirebase();
        firebase = firebaseContext.firebase;
        var messages = await GetMessages(firebase, prijateljstvaKey);
        messagesCollectionView.ItemsSource = messages;
    }

    private async void SendMessage_Clicked(object sender, EventArgs e)
    {
        if (prijateljstvaKey != null)
        {
            await AddMessage(firebase, prijateljstvaKey, uid1, messageEntry.Text);
            messageEntry.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Napaka!", "Prijateljstvo ni najdeno!", "OK");
        }
    }

    static async Task AddMessage(FirebaseClient firebase, string prijateljstvaKey, string senderUid, string text)
    {
        var message = new Message
        {
            Sender = senderUid,
            Text = text,
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")
        };

        await firebase.Child($"Prijateljstva/{prijateljstvaKey}/Messages").PostAsync(message);
    }

    static async Task<List<Message>> GetMessages(FirebaseClient firebase, string prijateljstvaKey)
    {
        try
        {
            var messages = await firebase.Child($"Prijateljstva/{prijateljstvaKey}/Messages").OnceAsync<Message>();
            return messages.Select(m => m.Object).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting messages: {ex.Message}");
            return new List<Message>();
        }
    }

    private async void BlitzchatClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainMenu());

    }

    private async void OnIzpisButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Pozor!", "Ste prepri?ani da se želite izpisati?", "Da", "Ne");

        if (result)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {

        }
    }

    private async void OnProfileTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnPrijateljiTabClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrijateljiPage());
    }
}