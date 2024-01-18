﻿using blitz_chat.Models;
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace blitz_chat;

public partial class DodajPrijateljaPage : ContentPage
{
    public FirebaseClient firebase;
    public static Dictionary<string, Uporabnik> Uporabniki = new Dictionary<string, Uporabnik>();
    public static Dictionary<string, Prijateljstva> Prijateljstva = new Dictionary<string, Prijateljstva>();
    public string uid;

    public DodajPrijateljaPage()
	{
        DisplayPrijateljstva();
        InitializeComponent();
        FirebaseContext firebaseContext = new FirebaseContext();
        firebaseContext.InitializeFirebase();
        firebase = firebaseContext.firebase;

        
    }
    public async Task DisplayPrijateljstva()
    {
        List<string> stringList = new List<string> { "String 1", "String 2", "String 3" };
        string UserID = await SecureStorage.Default.GetAsync("UserID");

        IFirebaseClient client = new FireSharp.FirebaseClient(new FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app/"
        });

        FirebaseResponse response = await client.GetAsync("Uporabniki");
        Uporabniki = response.ResultAs<Dictionary<string, Uporabnik>>();

        FirebaseResponse response2 = await client.GetAsync("Prijateljstva");
        Prijateljstva = response2.ResultAs<Dictionary<string, Prijateljstva>>();

        if (Prijateljstva != null)
        {
            foreach (var item in Prijateljstva)
            {
                string prijateljstvaKey = item.Key;
                Prijateljstva prijateljstvo = item.Value;

                if (prijateljstvo.IsFriendConfirmed == false & prijateljstvo.uid1 == UserID)
                {
                    foreach (var item2 in Uporabniki)
                    {
                        if (item2.Key.ToString() == prijateljstvo.uid2.ToString())
                        {
                            StackLayout labelContainer = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal
                            };


                            Label label = new Label
                            {
                                Text = item2.Value.Email.ToString(),
                                FontSize = 18
                            };


                            var image1 = new Image
                            {
                                Source = "add.png",
                                WidthRequest = 20,
                                HeightRequest = 20,
                                Margin = new Thickness(10, 0, 0, 0)
                            };
                            image1.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(async () =>
                                {
                                    Prijateljstva add = new Prijateljstva
                                    {
                                        uid1 = prijateljstvo.uid1,
                                        uid2 = prijateljstvo.uid2,
                                        IsFriendConfirmed = true,
                                    };

                                    try
                                    {
                                        var firebaseRoot = firebase.Child("Prijateljstva").Child(prijateljstvaKey);
                                        await firebaseRoot.PutAsync(add);
                                        this.labelContainer.Children.Remove(labelContainer);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error writing to Firebase: {ex.Message}");
                                    }


                                })
                            });


                            var image2 = new Image
                            {
                                Source = "decline.png",
                                WidthRequest = 30,
                                HeightRequest = 30,
                                Margin = new Thickness(10, 0, 0, 0)
                            };
                            image2.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(async () =>
                                {
                                    this.labelContainer.Children.Remove(labelContainer);
                                    await DeleteChildFromFirebase(prijateljstvaKey);
                                })
                            });


                            labelContainer.Children.Add(label);
                            labelContainer.Children.Add(image1);
                            labelContainer.Children.Add(image2);

                            this.labelContainer.Children.Add(labelContainer);
                        }
                    }
                }
            }
        }
    }

    private async Task DeleteChildFromFirebase(string uid)
    {
        try
        {
            var firebaseRoot = firebase.Child("Prijateljstva");

            var childReference = firebaseRoot.Child(uid);
            await childReference.DeleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting from Firebase: {ex.Message}");
        }
    }

    public async Task RegisterPrijateljstvoToFirebase(Prijateljstva prijateljstva)
    {
        try
        {
            var firebaseRoot = firebase.Child("Prijateljstva");
            await firebaseRoot.PostAsync(prijateljstva);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to Firebase: {ex.Message}");
        }
    }

    private async void DodajClicked(object sender, EventArgs e)
    {
        string UserID = await SecureStorage.Default.GetAsync("UserID");

        IFirebaseClient client = new FireSharp.FirebaseClient(new FirebaseConfig
        {
            AuthSecret = "AIzaSyB3fZ6kPDh-L9njqcFUwx7kKUxiOw0ElGE",
            BasePath = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app/"
        });

        FirebaseResponse response = await client.GetAsync("Uporabniki");
        Uporabniki = response.ResultAs<Dictionary<string, Uporabnik>>();

        FirebaseResponse response2 = await client.GetAsync("Prijateljstva");
        Prijateljstva = response2.ResultAs<Dictionary<string, Prijateljstva>>();

        foreach (var item in Uporabniki)
        {
            if(EmailEntry.Text != null)
            {
                if (item.Value.Email == EmailEntry.Text.ToString())
                {
                    uid = item.Key.ToString();
                }
            }
        }

        if(Prijateljstva != null)
        {
            foreach (var item in Prijateljstva.Values)
            {
                if (item.IsFriendConfirmed == false & item.uid1 == uid & item.uid2 == UserID || item.IsFriendConfirmed == false & item.uid1 == UserID & item.uid2 == uid)
                {
                    await DisplayAlert("Napaka!", "Ponovno pošiljanje prošnje ni mogoče!", "OK");
                    await Navigation.PushAsync(new DodajPrijateljaPage());
                    return;
                }
                else if (item.IsFriendConfirmed == true & item.uid1 == uid & item.uid2 == UserID || item.IsFriendConfirmed == true & item.uid1 == UserID & item.uid2 == uid)
                {
                    await DisplayAlert("Napaka!", "S to osebo ste že prijatelji!", "OK");
                    await Navigation.PushAsync(new DodajPrijateljaPage());
                    return;
                }
            }
        }


        if (EmailEntry.Text != null)
        {
            Prijateljstva prijateljstva = new Prijateljstva
            {
                uid1 = uid,
                uid2 = UserID,
                IsFriendConfirmed = false
            };
            if(prijateljstva.uid1 == prijateljstva.uid2)
            {
                await DisplayAlert("Napaka!", "Nemorete dodati samega sebe!", "OK");
            }
            else if(uid == null)
            {
                await DisplayAlert("Napaka!", "Uporabnik ni najden!", "OK");
            }
            else
            {
                await RegisterPrijateljstvoToFirebase(prijateljstva);
                await Navigation.PushAsync(new PrijateljiPage());
            }
        }
        
        else
        {
            await DisplayAlert("Napaka!", "Polja nesmejo biti prazna!", "OK");
        }
    }

    private async void OnIzpisButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Pozor!", "Se res želite izpisati?", "Da", "Ne");

        if (result)
        {
            //Uporabnika izpise, vrne na prijavo
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            //Nic se ne zgodi
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